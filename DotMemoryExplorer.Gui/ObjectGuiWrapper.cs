using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Gui {
	public class ObjectGuiWrapper : INotifyPropertyChanged, IDisposable {

		private Lazy<string> _objectBinaryContentLazy;
		private Lazy<ulong> _eeClassAddressLazy;
		private Lazy<ulong> _objectHeaderLazy;
		private Lazy<IEnumerable<FieldGuiWrapper>> _objectFieldsLazy;

		private ObjectDetailProvider _objectDetailProvider;
		private DotnetObjectMetadata _objectMetadata;
		private HeapDump _owningHeapDump;
		private ApplicationManager _appManager;

		public String Name {
			get {
				if (_appManager.LabelManager.HasObjectLabel(_objectMetadata)) {
					return _appManager.LabelManager.GetObjectLabel(_objectMetadata);
				} else {
					return DataTypeName;
				}
			}
		}

		public String Label {
			get {
				return _appManager.LabelManager.GetObjectLabel(_objectMetadata);
			}
		}

		public DotnetObjectMetadata ObjectMetadata {
			get {
				return _objectMetadata;
			}
		}

		public string DataTypeName {
			get {
				if (_appManager.LabelManager.HasDataTypeLabel(ObjectMetadata.TypeId)) {
					return _appManager.LabelManager.GetDataTypeLabel(ObjectMetadata.TypeId);
				} else {
					return _owningHeapDump.GetTypeById(ObjectMetadata.TypeId).TypeName;
				}
			}
		}
		public ulong MethodTableAddress {
			get {
				return ObjectMetadata.TypeId;
			}
		}

		public ulong EEClassAddress {
			get {
				return _eeClassAddressLazy.Value;
			}
		}

		public ulong ObjectHeader {
			get {
				return _objectHeaderLazy.Value;
			}
		}

		public string BinaryContentFormatted {
			get {
				return _objectBinaryContentLazy.Value;
			}
		}

		public IEnumerable<FieldGuiWrapper> Fields {
			get {
				return _objectFieldsLazy.Value;
			}
		}

		public ObjectGuiWrapper(DotnetObjectMetadata objectMetadata, HeapDump owningHeapDump, ApplicationManager appManager) {
			_objectMetadata = objectMetadata;
			_owningHeapDump = owningHeapDump;
			_appManager = appManager;
			_objectDetailProvider = new ObjectDetailProvider(objectMetadata, owningHeapDump);

			_objectBinaryContentLazy = new Lazy<string>(FormatBinaryContent);
			_eeClassAddressLazy = new Lazy<ulong>(ReadEEClassAddress);
			_objectHeaderLazy = new Lazy<ulong>(ReadObjectHeader);
			_objectFieldsLazy = new Lazy<IEnumerable<FieldGuiWrapper>>(ReadObjectFields);

			_appManager.LabelManager.ObjectLabelChanged += LabelManager_ObjectLabelChanged;
			_appManager.LabelManager.DataTypeLabelChanged += LabelManager_DataTypeLabelChanged;
		}

		private IEnumerable<FieldGuiWrapper> ReadObjectFields() {
			List<FieldGuiWrapper> fields = new List<FieldGuiWrapper>();
			foreach (var field in _objectDetailProvider.GetFields()) {
				fields.Add(new FieldGuiWrapper(field, _owningHeapDump, _appManager));
			}
			return fields;
		}

		private ulong ReadEEClassAddress() {
			return _objectDetailProvider.GetEEClassAddress();
		}

		private ulong ReadObjectHeader() {
			return _objectDetailProvider.GetObjectHeader();
		}

		private void LabelManager_ObjectLabelChanged(object? sender, ObjectLabelChangedEventArgs e) {
			if (e.ObjectAddress == _objectMetadata.Address) {
				RaisePropertyChanged(nameof(Name));
				RaisePropertyChanged(nameof(Label));
			}
		}

		private void LabelManager_DataTypeLabelChanged(object? sender, DataTypeLabelChangedEventArgs e) {
			if (e.DataTypeId == _objectMetadata.TypeId) {
				RaisePropertyChanged(nameof(DataTypeName));
			}
		}

		private string FormatBinaryContent() {
			try {
				return BuildBinaryContentFormatted();
			} catch (Exception ex) {
				return $"Error while reading memory. Details:\n\n{ex.GetType().Name}: {ex.Message}\n\n{ex.StackTrace}";
			}
		}

		private string BuildBinaryContentFormatted() {
			var mem = _owningHeapDump.MemoryDump.GetMemory(ObjectMetadata.Address, ObjectMetadata.Size);
			return BinaryDataFormatter.FormatBinary(mem);
		}

		private void RaisePropertyChanged(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public void Dispose() {
			_appManager.LabelManager.ObjectLabelChanged -= LabelManager_ObjectLabelChanged;
		}
	}
}
