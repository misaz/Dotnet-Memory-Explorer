using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Gui {

	public class LabelManager {

		private SortedDictionary<ulong, string> _objectLabel = new SortedDictionary<ulong, string>();
		private SortedDictionary<FieldId, string> _fieldLabels = new SortedDictionary<FieldId, string>();
		private SortedDictionary<ulong, string> _dataTypeLabels = new SortedDictionary<ulong, string>();

		public event EventHandler<ObjectLabelChangedEventArgs>? ObjectLabelChanged;
		public event EventHandler<DataTypeLabelChangedEventArgs>? DataTypeLabelChanged;
		public event EventHandler<FieldLabelChangedEventArgs>? FieldLabelChanged;

		public bool HasObjectLabel(DotnetObjectMetadata objectMetadata) {
			return HasObjectLabel(objectMetadata.Address);
		}
		public bool HasObjectLabel(ulong objectAddress) {
			return _objectLabel.ContainsKey(objectAddress);
		}
		public string GetObjectLabel(DotnetObjectMetadata objectMetadata) {
			return GetObjectLabel(objectMetadata.Address);
		}
		public string GetObjectLabel(ulong objectAddress) {
			if (_objectLabel.ContainsKey(objectAddress)) {
				return _objectLabel[objectAddress];
			} else {
				return string.Empty;
			}
		}
		public void SetObjectLabel(DotnetObjectMetadata objectMetadata, string label) {
			SetObjectLabel(objectMetadata.Address, label);
		}
		public void SetObjectLabel(ulong objectAddess, string label) {
			if (label == null) {
				throw new ArgumentNullException(nameof(label));
			}

			if (_objectLabel.ContainsKey(objectAddess)) {
				_objectLabel[objectAddess] = label;
			} else {
				_objectLabel.Add(objectAddess, label);
			}

			RaiseObjectLabelChanged(objectAddess);
		}
		private void RaiseObjectLabelChanged(ulong objectAddess) {
			if (ObjectLabelChanged != null) {
				ObjectLabelChanged(this, new ObjectLabelChangedEventArgs(objectAddess));
			}
		}

		public bool HasDataTypeLabel(DotnetTypeMetadata type) {
			return HasDataTypeLabel(type.TypeId);
		}
		public bool HasDataTypeLabel(ulong typeId) {
			return _dataTypeLabels.ContainsKey(typeId);
		}
		public string GetDataTypeLabel(DotnetTypeMetadata type) {
			return GetDataTypeLabel(type.TypeId);
		}
		public string GetDataTypeLabel(ulong typeId) {
			if (_dataTypeLabels.ContainsKey(typeId)) {
				return _dataTypeLabels[typeId];
			} else {
				return string.Empty;
			}
		}
		public void SetDataTypeLabel(DotnetTypeMetadata type, string label) {
			SetDataTypeLabel(type.TypeId, label);
		}
		public void SetDataTypeLabel(ulong typeId, string label) {
			if (_dataTypeLabels.ContainsKey(typeId)) {
				_dataTypeLabels[typeId] = label;
			} else {
				_dataTypeLabels.Add(typeId, label);
			}

			RaiseDataTypeLabelChanged(typeId);
		}
		private void RaiseDataTypeLabelChanged(ulong typeId) {
			if (DataTypeLabelChanged != null) {
				DataTypeLabelChanged(this, new DataTypeLabelChangedEventArgs(typeId));
			}
		}


		public string GetFieldLabel(FieldId fieldId) {
			if (_fieldLabels.ContainsKey(fieldId)) {
				return _fieldLabels[fieldId];
			} else {
				return string.Empty;
			}
		}
		public void SetFieldLabel(FieldId fieldId, string label) {
			if (_fieldLabels.ContainsKey(fieldId)) {
				_fieldLabels[fieldId] = label;
			} else {
				_fieldLabels.Add(fieldId, label);
			}
			RaiseFieldLabelChanged(fieldId);
		}
		private void RaiseFieldLabelChanged(FieldId fieldId) {
			if (FieldLabelChanged != null) {
				FieldLabelChanged(this, new FieldLabelChangedEventArgs(fieldId));
			}
		}
	}

}
