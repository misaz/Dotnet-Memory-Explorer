using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	public class ObjectDetailTab : Tab {
		public ObjectDetailTab(DotnetObjectMetadata obj, HeapDump owningHeapDump, ApplicationManager appManager) : base($"{HexadecimalAddressConverter.Shared.Convert(obj.Address)} ({owningHeapDump.GetTypeById(obj.TypeId).TypeName})", new ObjectDetailPane(obj, owningHeapDump, appManager), true) {
		}
	}
}
