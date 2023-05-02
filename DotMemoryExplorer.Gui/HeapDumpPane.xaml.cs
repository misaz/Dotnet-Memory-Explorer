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

		private HeapDumpViewModel _heapDumpViewModel;

		public HeapDumpPane(HeapDumpViewModel heapDumpViewModel) {
			if (heapDumpViewModel == null) {
				throw new ArgumentNullException(nameof(heapDumpViewModel));
			}

			_heapDumpViewModel = heapDumpViewModel;

			DataContext = _heapDumpViewModel;

			InitializeComponent();
		}
	}
}
