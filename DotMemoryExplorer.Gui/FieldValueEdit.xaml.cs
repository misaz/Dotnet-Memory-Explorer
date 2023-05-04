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
	/// Interaction logic for FieldValueEdit.xaml
	/// </summary>
	public partial class FieldValueEdit : Window {

		public string ValueString { get; set; }

		public FieldValueEdit(string oldValueString) {
			if (oldValueString == null) {
				throw new ArgumentNullException(nameof(oldValueString));
			}
			ValueString = oldValueString;

			DataContext = this;
			InitializeComponent();

			txtValue.Focus();
			txtValue.SelectAll();
		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			DialogResult = false;
		}

		private void Ok_Click(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}
	}
}
