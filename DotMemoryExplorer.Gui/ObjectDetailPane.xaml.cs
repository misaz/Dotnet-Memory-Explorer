using DotMemoryExplorer.Core;
using DotMemoryExplorer.Core.FieldValue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	public partial class ObjectDetailPane : UserControl, IDisposable {
		private ApplicationManager _appManager;
		private HeapDump _owningHeapDump;

		private ObjectsListingPane _referencesPane;
		private ObjectsListingPane _referencedByPane;

		public ObjectGuiWrapper Object { get; }

		public ObjectDetailPane(DotnetObjectMetadata objMetadata, HeapDump owningHeapDump, ApplicationManager appManager) {
			if (owningHeapDump == null) {
				throw new ArgumentNullException(nameof(owningHeapDump));
			}

			if (appManager == null) {
				throw new ArgumentNullException(nameof(appManager));
			}

			Object = new ObjectGuiWrapper(objMetadata, owningHeapDump, appManager);
			_owningHeapDump = owningHeapDump;
			_appManager = appManager;

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
				objects.Add(_owningHeapDump.GetObjectByAddress(reference.TargetObjectAddress));
			}
			return objects;
		}


		private void Field_DoubleClick(object sender, MouseButtonEventArgs e) {
			FieldGuiWrapper wrapper = GuiEventsHelper.UnpackSenderTag<FieldGuiWrapper>(sender);

			if (wrapper.FieldMetadata.Content is FieldValueClass) {
				FieldValueClass reference = (FieldValueClass)wrapper.FieldMetadata.Content;
				_appManager.OpenObjectDetail(reference.ReferencedObject, _owningHeapDump);
			}
		}

		private void EditObject_Click(object sender, RoutedEventArgs e) {
			var dlg = new NameEditDialog(_appManager.LabelManager.GetObjectLabel(Object.ObjectMetadata));
			if (dlg.ShowDialog() == true) {
				_appManager.LabelManager.SetObjectLabel(Object.ObjectMetadata, dlg.Label);
			}
		}

		public void Dispose() {
			Object.Dispose();
		}

		private void RenameField_Click(object sender, RoutedEventArgs e) {
			FieldGuiWrapper wrapper = GuiEventsHelper.UnpackSenderTag<FieldGuiWrapper>(sender);

			NameEditDialog dlg = new NameEditDialog(wrapper.Label);
			if (dlg.ShowDialog() == true) {
				_appManager.LabelManager.SetFieldLabel(wrapper.FieldMetadata.FieldId, dlg.Label);
			}
		}

		private void LabelDataType_Click(object sender, RoutedEventArgs e) {
			var dlg = new NameEditDialog(_appManager.LabelManager.GetDataTypeLabel(Object.ObjectMetadata.TypeId));
			if (dlg.ShowDialog() == true) {
				_appManager.LabelManager.SetDataTypeLabel(Object.ObjectMetadata.TypeId, dlg.Label);
			}
		}

		private void LabelObject_Click(object sender, RoutedEventArgs e) {
			var dlg = new NameEditDialog(_appManager.LabelManager.GetObjectLabel(Object.ObjectMetadata));
			if (dlg.ShowDialog() == true) {
				_appManager.LabelManager.SetObjectLabel(Object.ObjectMetadata, dlg.Label);
			}
		}

		private void ChangeValue_Click(object sender, RoutedEventArgs e) {
			FieldGuiWrapper wrapper = GuiEventsHelper.UnpackSenderTag<FieldGuiWrapper>(sender);

			if (wrapper.FieldMetadata.Content is FieldValueClass) {
				FieldValueClass value = (FieldValueClass)wrapper.FieldMetadata.Content;
				UpdateReferenceField(wrapper.FieldMetadata, value);
			} else if (wrapper.FieldMetadata.Content is IEditableFieldValue) {
				IEditableFieldValue value = (IEditableFieldValue)wrapper.FieldMetadata.Content;
				UpdateValueField(wrapper.FieldMetadata, value);
			} else {
				MessageBox.Show("Updating specified field is not supported", "Not supported", MessageBoxButton.OK, MessageBoxImage.Information);
			}

		}

		private void UpdateValueField(FieldMetadata meta, IEditableFieldValue value) {
			var dlg = new FieldValueEdit(value.BuildValueString());
			if (dlg.ShowDialog() == true) {
				if (value.IsValid(dlg.ValueString)) {
					byte[] writeMemory = value.GetValueBytes(dlg.ValueString);
					PatchFieldValue(meta, writeMemory);
				} else {
					MessageBox.Show("Unable to parse entered value.", "Parse error", MessageBoxButton.OK, MessageBoxImage.Error);
				}
			}
		}

		private void UpdateReferenceField(FieldMetadata meta, FieldValueClass fvc) {
			var objSelector = new ObjectSelector(fvc.ReferencedObjectType.TypeId, _owningHeapDump, _appManager);
			if (objSelector.ShowDialog() == true && objSelector.SelectedObject != null) {
				byte[] patchedMemoryContent;
				if (_owningHeapDump.OwningProcess.Bitness == 64) {
					patchedMemoryContent = BitConverter.GetBytes(objSelector.SelectedObject.ObjectMetadata.Address);
				} else {
					patchedMemoryContent = BitConverter.GetBytes((uint)objSelector.SelectedObject.ObjectMetadata.Address);
				}

				PatchFieldValue(meta, patchedMemoryContent);
			}
		}

		private void PatchFieldValue(FieldMetadata field, byte[] patchedMemoryContent) {
			try {
				ulong compareAddress = Object.ObjectMetadata.Address;
				ReadOnlySpan<byte> compareMemory = Object.ObjectMemoryContent;
				ulong writeAddress = Object.GetFieldContentAddress(field);
				byte[] writeMemory = patchedMemoryContent;

				_owningHeapDump.PatchMemory(compareAddress, compareMemory, writeAddress, writeMemory);
			} catch (ComparisonFailedException) {
				MessageBox.Show($"Object was relocated since the heap dump was created and is no more patchable at it's original address.", "Patch error", MessageBoxButton.OK, MessageBoxImage.Error);
			} catch (Exception ex) {
				MessageBox.Show($"Error while patching memory. Details:\n\n{ex.GetType().Name}: {ex.Message}", "Patch error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
