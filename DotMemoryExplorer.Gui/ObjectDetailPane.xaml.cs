using DotMemoryExplorer.Core;
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

		private ObjectDetailProvider _objectDetailProvider;
		private Lazy<string> _typeNameLazy;
		private Lazy<string> _objectBinaryContentLazy;
		private Lazy<ulong> _eeClassAddressLazy;
		private Lazy<IEnumerable<FieldMetadata>> _objectFieldsLazy;

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

			_typeNameLazy = new Lazy<string>(ResolveTypeName);
			_objectBinaryContentLazy = new Lazy<string>(FormatBinaryContent);
			_eeClassAddressLazy = new Lazy<ulong>(ReadEEClassAddress);
			_objectFieldsLazy = new Lazy<IEnumerable<FieldMetadata>>(ReadObjectFields);
			_objectDetailProvider = new ObjectDetailProvider(objMetadata, owningHeapDump);
			Metadata = objMetadata;
			OwningHeapDump = owningHeapDump;

			DataContext = this;
			InitializeComponent();
		}

		private IEnumerable<FieldMetadata> ReadObjectFields() {
			return _objectDetailProvider.GetFields();
		}

		private ulong ReadEEClassAddress() {
			return _objectDetailProvider.GetEEClassAddress();
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

	}
}
