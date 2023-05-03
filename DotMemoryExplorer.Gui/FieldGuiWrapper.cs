using DotMemoryExplorer.Core;
using DotMemoryExplorer.Core.FieldValue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Gui {
	public class FieldGuiWrapper : INotifyPropertyChanged {

		private ObjectGuiWrapper? _referencedValue = null;
		private FieldMetadata _fieldMetadata;
		private HeapDump _owningHeapDump;
		private ApplicationManager _appManager;

		public event PropertyChangedEventHandler? PropertyChanged;

		public String Label {
			get {
				return _appManager.LabelManager.GetFieldLabel(_fieldMetadata.FieldId);
			}
		}

		public FieldMetadata FieldMetadata {
			get {
				return _fieldMetadata;
			}
		}

		public string OwningClass {
			get {
				ulong typeId = _fieldMetadata.FieldId.OwnerTypeId;
				if (_appManager.LabelManager.HasDataTypeLabel(typeId)) {
					return _appManager.LabelManager.GetDataTypeLabel(typeId);
				} else {
					return _owningHeapDump.GetTypeById(typeId).TypeName;
				}
			}
		}

		public string ValueString {
			get {
				if (_referencedValue == null) {
					return $"{FieldMetadata.Content}";
				} else {
					if (_appManager.LabelManager.HasObjectLabel(_referencedValue.ObjectMetadata)) {
						return _appManager.LabelManager.GetObjectLabel(_referencedValue.ObjectMetadata);
					} else {
						string addr = HexadecimalAddressConverter.Shared.Convert(_referencedValue.ObjectMetadata.Address);
						string dt;
						if (_appManager.LabelManager.HasDataTypeLabel(_referencedValue.ObjectMetadata.TypeId)) {
							dt = _appManager.LabelManager.GetDataTypeLabel(_referencedValue.ObjectMetadata.TypeId);
						} else {
							dt = _owningHeapDump.GetTypeById(_referencedValue.ObjectMetadata.TypeId).TypeName;
						}
						return $"{addr} ({dt})";
					}
				}
			}
		}

		public FieldGuiWrapper(FieldMetadata fieldMetadata, HeapDump owningHeapDump, ApplicationManager appManager) {
			_fieldMetadata = fieldMetadata;
			_owningHeapDump = owningHeapDump;
			_appManager = appManager;

			if (_fieldMetadata.Content is FieldValueClass) {
				FieldValueClass val = (FieldValueClass)_fieldMetadata.Content;
				_referencedValue = new ObjectGuiWrapper(val.ReferencedObject, owningHeapDump, appManager);
			}

			_appManager.LabelManager.FieldLabelChanged += LabelManager_FieldLabelChanged;
			_appManager.LabelManager.ObjectLabelChanged += LabelManager_ObjectLabelChanged;
			_appManager.LabelManager.DataTypeLabelChanged += LabelManager_DataTypeLabelChanged;
		}

		private void LabelManager_DataTypeLabelChanged(object? sender, DataTypeLabelChangedEventArgs e) {
			if (_referencedValue != null && _referencedValue.ObjectMetadata.TypeId == e.DataTypeId) {
				RaisePropertyChanged(nameof(ValueString));
			}

			if (_fieldMetadata.FieldId.OwnerTypeId == e.DataTypeId) {
				RaisePropertyChanged(nameof(OwningClass));
			}
		}

		private void LabelManager_ObjectLabelChanged(object? sender, ObjectLabelChangedEventArgs e) {
			if (_referencedValue != null && _referencedValue.ObjectMetadata.Address == e.ObjectAddress) {
				RaisePropertyChanged(nameof(ValueString));
			}
		}

		private void LabelManager_FieldLabelChanged(object? sender, FieldLabelChangedEventArgs e) {
			if (e.FieldId.AreSame(_fieldMetadata.FieldId)) {
				RaisePropertyChanged(nameof(Label));
			}
		}

		private void RaisePropertyChanged(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
