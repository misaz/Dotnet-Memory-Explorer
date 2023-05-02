using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	public class ObjectsListingTab : Tab {
		public ObjectsListingTab(string name, IEnumerable<DotnetObjectMetadata> objects, HeapDump owningHeapDump, ApplicationManager appManager) : base(name, new ObjectsListingPane(objects, owningHeapDump, appManager), true) {
		}
	}
}
