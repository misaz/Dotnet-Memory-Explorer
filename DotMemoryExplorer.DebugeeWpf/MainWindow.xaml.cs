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

namespace DotMemoryExplorer.DebugeeWpf {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private List<TestItem> SavedItems = new List<TestItem>();

		public MainWindow() {
			InitializeComponent();
		}

		private void Add_Click(object sender, RoutedEventArgs e) {
			string text = txtText.Text;
			int value;
			try {
				value = int.Parse(txtValue.Text);
			} catch (Exception) {
				MessageBox.Show("Item value must be int number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			TestItem? reference;
			if (cmbReference.SelectedItem == cmbReferenceFirst) {
				reference = SavedItems.FirstOrDefault();
			} else if (cmbReference.SelectedItem == cmbReferenceSecond) {
				if (SavedItems.Count >= 2) {
					reference = SavedItems[1];
				} else {
					reference = null;
				}
			} else if (cmbReference.SelectedItem == cmbReferenceLast) {
				reference = SavedItems.LastOrDefault();
			} else {
				reference = null;
			}

			SavedItems.Add(new TestItem() { Name = txtText.Text, Value = value, Reference = reference });
		}

		private void Print_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show(string.Join("\n", SavedItems.Select(x => FormatItem(x))));
		}

		private object FormatItem(TestItem testItem) {
			string referenceText;
			if (testItem.Reference != null) {
				referenceText = $"#{SavedItems.IndexOf(testItem.Reference)}";
			} else {
				referenceText = "null";
			}
			return $"{testItem.Name} with value {testItem.Value} and ref to {referenceText}";
		}
	}
}
