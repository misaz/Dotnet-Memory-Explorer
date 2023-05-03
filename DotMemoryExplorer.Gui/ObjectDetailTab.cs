using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	public class ObjectDetailTab : Tab {

		private DotnetObjectMetadata _metadata;
		private HeapDump _owningHeapDump;
		private ApplicationManager _appManager;

		public ObjectDetailTab(DotnetObjectMetadata obj, HeapDump owningHeapDump, ApplicationManager appManager) : base(string.Empty, new ObjectDetailPane(obj, owningHeapDump, appManager), true) {
			_metadata = obj;
			_owningHeapDump = owningHeapDump;
			_appManager = appManager;

			appManager.LabelManager.ObjectLabelChanged += LabelManager_ObjectLabelChanged;

			UpdateTabName();
		}

		private void LabelManager_ObjectLabelChanged(object? sender, ObjectLabelChangedEventArgs e) {
			if (e.ObjectAddress == _metadata.Address) {
				UpdateTabName();
			}
		}

		private void UpdateTabName() {
			if (_appManager.LabelManager.HasObjectLabel(_metadata)) {
				Name = _appManager.LabelManager.GetObjectLabel(_metadata);
			} else {
				Name = $"{HexadecimalAddressConverter.Shared.Convert(_metadata.Address)} ({_owningHeapDump.GetTypeById(_metadata.TypeId).TypeName})";
			} 
		}
	}
}
