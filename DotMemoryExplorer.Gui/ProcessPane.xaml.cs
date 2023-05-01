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
	/// Interaction logic for ProcessPane.xaml
	/// </summary>
	public partial class ProcessPane : UserControl {

		private readonly IDotnetProcess _process;
		private readonly ApplicationManager _appManager;
		private IEnumerable<MemoryAddressRange> _processMappedMemoryRegions;

		public IDotnetProcess Process {
			get {
				return _process;
			}
		}

		public IEnumerable<MemoryAddressRange> ProcessMappedMemoryRegions {
			get {
				return _processMappedMemoryRegions;
			}
		}

		public ProcessPane(IDotnetProcess process, ApplicationManager appManager) {
			if (process == null) {
				throw new ArgumentNullException(nameof(process));
			}

			_process = process;
			_appManager = appManager;
			_processMappedMemoryRegions = _process.ProcessMemoryManger.GetMemoryRegions();

			DataContext = this;
			InitializeComponent();
		}

		private void DumpHeap_Click(object sender, RoutedEventArgs e) {
		}
	}
}
