using DotMemoryExplorer.Core;
using DotMemoryExplorer.Core.FieldValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DotMemoryExplorer.Gui {
	/// <summary>
	/// Interaction logic for ObjectDetailPane.xaml
	/// </summary>
	public partial class ObjectDetailPane : UserControl {
		private ApplicationManager _appManager;
		private ObjectDetailProvider _objectDetailProvider;
		private Lazy<string> _typeNameLazy;
		private Lazy<string> _objectBinaryContentLazy;
		private Lazy<ulong> _eeClassAddressLazy;
		private Lazy<ulong> _objectHeaderLazy;
		private Lazy<IEnumerable<FieldMetadata>> _objectFieldsLazy;
		private ObjectsListingPane _referencesPane;
		private ObjectsListingPane _referencedByPane;

		public HeapDump OwningHeapDump { get; }
		public DotnetObjectMetadata Metadata { get; }

		public string TypeName {
			get {
				return _typeNameLazy.Value;
			}
		}

		public ulong MethodTableAddress {
			get {
				return Metadata.TypeId;
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

		public IEnumerable<FieldMetadata> Fields {
			get {
				return _objectFieldsLazy.Value;
			}
		}

		public ObjectDetailPane(DotnetObjectMetadata objMetadata, HeapDump owningHeapDump, ApplicationManager appManager) {
			if (owningHeapDump == null) {
				throw new ArgumentNullException(nameof(owningHeapDump));
			}

			if (appManager == null) {
				throw new ArgumentNullException(nameof(appManager));
			}

			Metadata = objMetadata;
			OwningHeapDump = owningHeapDump;
			_appManager = appManager;
			_typeNameLazy = new Lazy<string>(ResolveTypeName);
			_objectBinaryContentLazy = new Lazy<string>(FormatBinaryContent);
			_eeClassAddressLazy = new Lazy<ulong>(ReadEEClassAddress);
			_objectHeaderLazy = new Lazy<ulong>(ReadObjectHeader);
			_objectFieldsLazy = new Lazy<IEnumerable<FieldMetadata>>(ReadObjectFields);
			_objectDetailProvider = new ObjectDetailProvider(objMetadata, owningHeapDump);
			_referencesPane = new ObjectsListingPane(LoadReferences(objMetadata.References), owningHeapDump, _appManager);
			_referencedByPane = new ObjectsListingPane(LoadReferences(objMetadata.ReferencedBy), owningHeapDump, _appManager);

			DataContext = this;
			InitializeComponent();

			tabReferences.Content = _referencesPane;
			tabReferencedBy.Content = _referencedByPane;
		}

		private IEnumerable<DotnetObjectMetadata> LoadReferences(IEnumerable<DotnetReferenceMetadata> references) {
			List<DotnetObjectMetadata> objects = new List<DotnetObjectMetadata>(references.Count());
			foreach (var reference in references) {
				objects.Add(OwningHeapDump.GetObjectByAddress(reference.TargetObjectAddress));
			}
			return objects;
		}

		private IEnumerable<FieldMetadata> ReadObjectFields() {
			return _objectDetailProvider.GetFields();
		}

		private ulong ReadEEClassAddress() {
			return _objectDetailProvider.GetEEClassAddress();
		}
		private ulong ReadObjectHeader() {
			return _objectDetailProvider.GetObjectHeader();
		}

		private string ResolveTypeName() {
			return OwningHeapDump.GetTypeById(Metadata.TypeId).TypeName;
		}

		private string BuildBinaryContentFormatted() {
			var mem = OwningHeapDump.MemoryDump.GetMemory(Metadata.Address, Metadata.Size);
			return BinaryDataFormatter.FormatBinary(mem);
		}

		private string FormatBinaryContent() {
			try {
				return BuildBinaryContentFormatted();
			} catch (Exception ex) {
				return $"Error while reading memory. Details:\n\n{ex.GetType().Name}: {ex.Message}\n\n{ex.StackTrace}";
			}
		}

		private void Field_DoubleClick(object sender, MouseButtonEventArgs e) {
			if (sender is not Control) {
				throw new InvalidOperationException("Cannot process event because it is triggered by non-control.");
			}

			Control c = (Control)sender;

			if (c.Tag is not FieldMetadata) {
				throw new InvalidOperationException("Cannot process event because tag of control triggering event is not send to field metadata.");
			}

			FieldMetadata meta = GuiEventsHelper.UnpackSenderTag<FieldMetadata>(sender);

			if (meta.Content is FieldValueClass) {
				FieldValueClass reference = (FieldValueClass)meta.Content;
				_appManager.OpenObjectDetail(reference.ReferencedObject, OwningHeapDump);
			}
		}
    }
}
