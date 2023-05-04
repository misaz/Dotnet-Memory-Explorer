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
using System.Windows.Shapes;

namespace DotMemoryExplorer.Gui {
	/// <summary>
	/// Interaction logic for ObjectSelector.xaml
	/// </summary>
	public partial class ObjectSelector : Window, INotifyPropertyChanged {
		private ObjectGuiWrapper? _selectedObject = null;
		private List<ObjectGuiWrapper> _objects = new List<ObjectGuiWrapper>();
		private ulong _dataTypeId;
		private HeapDump _heapDump;

		public ObjectSelector(ulong dataTypeId, HeapDump heapDump, ApplicationManager appManager) {
			_dataTypeId = dataTypeId;
			_heapDump = heapDump;

			var objects = heapDump.DataTypeObjectGrouping.GetObjectsByTypeId(dataTypeId);
			foreach ( var obj in objects ) {
				_objects.Add(new ObjectGuiWrapper(obj, heapDump, appManager));
			}

			DataContext = this;
			InitializeComponent();

			if (appManager.LabelManager.HasDataTypeLabel(dataTypeId)) {
				Title = appManager.LabelManager.GetDataTypeLabel(dataTypeId);
			} else {
				Title = heapDump.GetTypeById(dataTypeId).TypeName;
			}
		}

		public IEnumerable<ObjectGuiWrapper> Objects {
			get {
				return _objects.AsReadOnly();
			}
		}

		public ObjectGuiWrapper? SelectedObject {
			get {
				return _selectedObject;
			}
			set {
				if (value == null) {
					throw new ArgumentNullException(nameof(value));
				}

				_selectedObject = value;

				RaisePropertyChanged(nameof(SelectedObject));
				RaisePropertyChanged(nameof(SelectEnabled));
			}
		}

		public bool SelectEnabled {
			get {
				return SelectedObject != null;
			}
		}


		private void Cancel_Click(object sender, RoutedEventArgs e) {
			DialogResult = false;
        }

		private void Ok_Click(object sender, RoutedEventArgs e) {
			if (!SelectEnabled) {
				throw new InvalidOperationException("Unabale to confirm dialog because no object were selected.");
			}

			DialogResult = true;
        }

		public event PropertyChangedEventHandler? PropertyChanged;

		private void RaisePropertyChanged(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
