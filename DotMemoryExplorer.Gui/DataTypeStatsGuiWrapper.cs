using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Gui {
	public class DataTypeStatsGuiWrapper : INotifyPropertyChanged {

		private HeapDump _owningDump;
		private ApplicationManager _appManager;

		public event PropertyChangedEventHandler? PropertyChanged;

		private DataTypeStatisticsEntry _statsEntry { get; }


		public DataTypeStatisticsEntry StatsEntry {
			get {
				return _statsEntry;
			}
		}

		public string TypeName {
			get {
				if (_appManager.LabelManager.HasDataTypeLabel(_statsEntry.Type)) {
					return _appManager.LabelManager.GetDataTypeLabel(_statsEntry.Type);
				} else {
					return _statsEntry.Type.TypeName;
				}
			}
		}

		public DataTypeStatsGuiWrapper(DataTypeStatisticsEntry dataTypeStatisticsEntry, HeapDump owningDump, ApplicationManager appManager) {
			if (owningDump == null) {
				throw new ArgumentNullException(nameof(owningDump));
			}

			if (appManager == null) {
				throw new ArgumentNullException(nameof(appManager));
			}

			_statsEntry = dataTypeStatisticsEntry;
			_owningDump = owningDump;
			_appManager = appManager;

			_appManager.LabelManager.DataTypeLabelChanged += LabelManager_DataTypeLabelChanged;
		}

		private void LabelManager_DataTypeLabelChanged(object? sender, DataTypeLabelChangedEventArgs e) {
			if (e.DataTypeId == _statsEntry.Type.TypeId) {
				RaisePropertyChanged(nameof(TypeName));
			}
		}

		private void RaisePropertyChanged(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
