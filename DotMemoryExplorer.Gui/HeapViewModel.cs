using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Gui {

	public class HeapDumpViewModel : INotifyPropertyChanged {

		private string _dataTypeSearchTerm = string.Empty;
		private IEnumerable<DataTypeStatisticsEntry> _filteredDataTypeStats;
		private HeapDump _heap;

		public event PropertyChangedEventHandler? PropertyChanged;

		public HeapDump HeapDump {
			get {
				return _heap;
			}
		}

		public IEnumerable<DataTypeStatisticsEntry> FilteredDataTypeStats {
			get {
				return _filteredDataTypeStats;
			}
		}

		public string DataTypeFilterSearch { 
			get {
				return _dataTypeSearchTerm;
			}
			set {
			if (value == null) {
					throw new ArgumentNullException(nameof(value));
				}

				_dataTypeSearchTerm = value;
				FilterStatistics();
				RaisePropertyChanged(nameof(FilteredDataTypeStats));
			}
		}

		private void RaisePropertyChanged(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			} 
		}

		private void FilterStatistics() {
			if (string.IsNullOrWhiteSpace(_dataTypeSearchTerm)) {
				_filteredDataTypeStats = _heap.DataTypeObjectGrouping.Statistics;
			} else {
				string termLower = _dataTypeSearchTerm.ToLowerInvariant();
				_filteredDataTypeStats = from x in _heap.DataTypeObjectGrouping.Statistics
										 where x.Type.TypeName.ToLowerInvariant().Contains(termLower)
										 select x;
			}
		}

		public HeapDumpViewModel(HeapDump heap) {
			if (heap == null) {
				throw new ArgumentNullException(nameof(heap));
			}

			_heap = heap;
			_filteredDataTypeStats = _heap.DataTypeObjectGrouping.Statistics;
		}

	}
}
