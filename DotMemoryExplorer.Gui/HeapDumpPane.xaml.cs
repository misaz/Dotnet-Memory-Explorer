using DotMemoryExplorer.Core;
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
	/// Interaction logic for HeapDumpPane.xaml
	/// </summary>
	public partial class HeapDumpPane : UserControl, INotifyPropertyChanged {

		private HeapDump _heapDump;
		private ApplicationManager _appManager;

		private string _dataTypeSearchTerm = string.Empty;
		private IEnumerable<DataTypeStatsGuiWrapper> _allStats;
		private IEnumerable<DataTypeStatsGuiWrapper> _filderedStats;

		public IEnumerable<DataTypeStatsGuiWrapper> FilteredDataTypeStats {
			get {
				return _filderedStats;
			}
		}

		public HeapDump HeapDump {
			get {
				return _heapDump;
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

		public HeapDumpPane(HeapDump dump, ApplicationManager appManager) {
			if (dump == null) {
				throw new ArgumentNullException(nameof(dump));
			}

			if (appManager == null) {
				throw new ArgumentNullException(nameof(appManager));
			}

			_heapDump = dump;
			_appManager = appManager;
			_allStats = CreateStatsWrappers(dump.DataTypeObjectGrouping.Statistics);
			_filderedStats = _allStats;

			_appManager.LabelManager.DataTypeLabelChanged += LabelManager_DataTypeLabelChanged;

			DataContext = this;
			InitializeComponent();
		}

		private void LabelManager_DataTypeLabelChanged(object? sender, DataTypeLabelChangedEventArgs e) {
			FilterStatistics();
			RaisePropertyChanged(nameof(FilteredDataTypeStats));
		}

		private IEnumerable<DataTypeStatsGuiWrapper> CreateStatsWrappers(IEnumerable<DataTypeStatisticsEntry> statistics) {
			List<DataTypeStatsGuiWrapper> output = new List<DataTypeStatsGuiWrapper>();

			foreach (DataTypeStatisticsEntry entry in statistics) {
				output.Add(new DataTypeStatsGuiWrapper(entry, _heapDump, _appManager));
			}

			return output;
		}

		private void DataType_DoubleClick(object sender, MouseButtonEventArgs e) {
			DataTypeStatsGuiWrapper wrapper = GuiEventsHelper.UnpackSenderTag<DataTypeStatsGuiWrapper>(sender);
			var objects = _heapDump.DataTypeObjectGrouping.GetObjectsByTypeId(wrapper.StatsEntry.Type.TypeId);
			_appManager.AddTab(new ObjectsListingTab($"{wrapper.StatsEntry.Type.TypeName} Instances", objects, _heapDump, _appManager));
        }


		public event PropertyChangedEventHandler? PropertyChanged;


		private void RaisePropertyChanged(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private void FilterStatistics() {
			if (string.IsNullOrWhiteSpace(_dataTypeSearchTerm)) {
				_filderedStats = _allStats;
			} else {
				string termLower = _dataTypeSearchTerm.ToLowerInvariant();
				_filderedStats = from x in _allStats
										 where x.TypeName.ToLowerInvariant().Contains(termLower)
										 select x;
			}
		}

	}
}
