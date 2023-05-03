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
	public partial class ObjectsListingPane : UserControl, IDisposable {

		private readonly ApplicationManager _applicationManager;
		private List<ObjectGuiWrapper> _objects;

		public IEnumerable<ObjectGuiWrapper> Objects {
			get {
				return _objects.AsReadOnly();
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

			_objects = new List<ObjectGuiWrapper>();
			foreach (var obj in objects) {
				_objects.Add(new ObjectGuiWrapper(obj, heapDump, appManager));
			}

			_applicationManager = appManager;
			HeapDump = heapDump;

			DataContext = this;
			InitializeComponent();
		}

		private void Object_DoubleClick(object sender, MouseButtonEventArgs e) {
			ObjectGuiWrapper wrapper = GuiEventsHelper.UnpackSenderTag<ObjectGuiWrapper>(sender);
			_applicationManager.OpenObjectDetail(wrapper.ObjectMetadata, HeapDump);
        }

		public void Dispose() {
			foreach (var obj in _objects) {
				obj.Dispose();
			}
			_objects.Clear();
		}
	}
}
