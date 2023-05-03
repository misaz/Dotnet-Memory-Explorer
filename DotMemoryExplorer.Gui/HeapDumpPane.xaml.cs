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
	/// Interaction logic for HeapDumpPane.xaml
	/// </summary>
	public partial class HeapDumpPane : UserControl {

		private ApplicationManager _appManager;
		private HeapDumpViewModel _heapDumpViewModel;

		public HeapDumpPane(HeapDumpViewModel heapDumpViewModel, ApplicationManager appManager) {
			if (heapDumpViewModel == null) {
				throw new ArgumentNullException(nameof(heapDumpViewModel));
			}

			if (appManager == null) {
				throw new ArgumentNullException(nameof(appManager));
			}

			_heapDumpViewModel = heapDumpViewModel;
			_appManager = appManager;

			DataContext = _heapDumpViewModel;

			InitializeComponent();
		}

		private void DataType_DoubleClick(object sender, MouseButtonEventArgs e) {
			DataTypeStatisticsEntry statEntry = GuiEventsHelper.UnpackSenderTag<DataTypeStatisticsEntry>(sender);
			var objects = _heapDumpViewModel.HeapDump.DataTypeObjectGrouping.GetObjectsByTypeId(statEntry.Type.TypeId);
			_appManager.AddTab(new ObjectsListingTab($"{statEntry.Type.TypeName} Instances", objects, _heapDumpViewModel.HeapDump, _appManager));
        }

    }
}
