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
using System.Windows.Shapes;

namespace DotMemoryExplorer.Gui
{
    /// <summary>
    /// Interaction logic for ProcessSelectDialog.xaml
    /// </summary>
    public partial class ProcessSelectDialog : Window
    {

        private ProcessSelectDataContext _dataContext = new ProcessSelectDataContext();

        private ProcessMetadata? _selectedProcess;

        public ProcessMetadata? SelectedProcess {
            get {
                return _selectedProcess;
            }
        }

        public ProcessSelectDialog()
        {
            this.DataContext = _dataContext;
            _selectedProcess = null;
			InitializeComponent();
        }

		private void Refresh_Click(object sender, RoutedEventArgs e) {
            _dataContext.RefreshProcesses();
		}

		private void Ok_Click(object? sender, RoutedEventArgs? e) {
            _selectedProcess = _dataContext.SelectedProcess;
            DialogResult = true;
        }

		private void Cancel_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }

		private void Proces_DoubleClick(object sender, MouseButtonEventArgs e) {
			_selectedProcess = _dataContext.SelectedProcess;
			DialogResult = true;
		}

	}
}
