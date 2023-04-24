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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private ApplicationManager _appManager = new ApplicationManager();

		public MainWindow() {
			this.DataContext = _appManager;
			InitializeComponent();
		}

		/// <summary>
		/// This is event handler used in close button of each tab. Tab instance has to bind into Tag property of button firing this event.
		/// </summary>
		private void CloseSenderTagTab(object sender, RoutedEventArgs e) {
			_appManager.CloseTab((Tab)((Button)sender).Tag);
		}

	}
}
