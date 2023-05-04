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
		private byte[]? _objectMemoryContent = null;
		private IEnumerable<FieldGuiWrapper>? _objectFields = null;

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
				if (_objectFields == null) {
					_objectFields = ReadObjectFields();
				}
				return _objectFields;
			}
		}

		public ReadOnlySpan<byte> ObjectMemoryContent {
			get {
				if (_objectMemoryContent == null) {
					_objectMemoryContent = ReadObjectMemoryContent();
				}
				return _objectMemoryContent;
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

			_appManager.LabelManager.ObjectLabelChanged += LabelManager_ObjectLabelChanged;
			_appManager.LabelManager.DataTypeLabelChanged += LabelManager_DataTypeLabelChanged;
			_owningHeapDump.MemoryDump.MemoryPatched += MemoryDump_MemoryPatched;
		}

		private void MemoryDump_MemoryPatched(object? sender, MemoryPatchedEventArgs e) {
			_objectMemoryContent = null;
			_objectFields = null;
			RaisePropertyChanged(nameof(Fields));
			RaisePropertyChanged(nameof(ObjectMemoryContent));
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

		private byte[] ReadObjectMemoryContent() {
			return _owningHeapDump.MemoryDump.GetMemory(ObjectMetadata.Address, ObjectMetadata.Size).ToArray();
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
			return BinaryDataFormatter.FormatBinary(ObjectMemoryContent);
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

		public ulong GetFieldContentAddress(FieldMetadata fieldMetadata) {
			return ObjectMetadata.Address + 8 + fieldMetadata.Offset;
		}
	}
}
