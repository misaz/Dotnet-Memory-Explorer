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
using System.Windows.Shapes;

namespace DotMemoryExplorer.Gui {
	/// <summary>
	/// Interaction logic for SearchStringWindow.xaml
	/// </summary>
	public partial class SearchStringWindow : Window {

		public SearchStringRequest? Request { get; private set; }

		public SearchStringWindow() {
			InitializeComponent();
		}

		private void Search_Click(object sender, RoutedEventArgs e) {
			Encoding enc;

			if (radioUtf8.IsChecked == true) {
				enc = Encoding.UTF8;
			} else if (radioUtf16.IsChecked == true) {
				enc = Encoding.Unicode;
			} else if (radioAscii.IsChecked == true) {
				enc = Encoding.ASCII;
			} else {
				enc = Encoding.Unicode;
			}

			Request = new SearchStringRequest(txtSearch.Text, enc);
			DialogResult = true;
        }

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			DialogResult = false;
        }
    }
}
