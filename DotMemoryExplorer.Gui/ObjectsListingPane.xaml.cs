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
	/// Interaction logic for ObjectsListingPane.xaml
	/// </summary>
	public partial class ObjectsListingPane : UserControl {

		private IEnumerable<DotnetObjectMetadata> _objects;

		public IEnumerable<DotnetObjectMetadata> Objects {
			get {
				return _objects;
			}
		}

		public HeapDump HeapDump { get; }

		public ObjectsListingPane(IEnumerable<DotnetObjectMetadata> objects, HeapDump heapDump, ApplicationManager appManager) {
			if (objects == null) {
				throw new ArgumentNullException(nameof(objects));
			}

			if (heapDump == null) {
				throw new ArgumentNullException(nameof(heapDump));
			}

			if (appManager == null) {
				throw new ArgumentNullException(nameof(appManager));
			}

			_objects = objects;
			HeapDump = heapDump;

			DataContext = this;
			InitializeComponent();
		}

		private void Object_DoubleClick(object sender, MouseButtonEventArgs e) {

        }
    }
}
