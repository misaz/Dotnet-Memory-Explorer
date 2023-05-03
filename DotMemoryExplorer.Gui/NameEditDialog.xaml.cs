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
	/// Interaction logic for NameEditDialog.xaml
	/// </summary>
	public partial class NameEditDialog : Window {

		public string Label { get; set; }

		public NameEditDialog(string label) {
			if (label == null) {
				throw new ArgumentNullException(nameof(label));
			}
			Label = label;

			DataContext = this;
			InitializeComponent();

			txtLabel.Focus();
			txtLabel.SelectAll();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			DialogResult = false;
		}

		private void Ok_Click(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}
	}
}
