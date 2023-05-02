using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	internal class HeapDumpTab : Tab {

		public HeapDumpTab(int dumpNumber, HeapDump dump, ApplicationManager appManager) : base($"Dump #{dumpNumber} of {dump.OwningProcess.ToString()}", new HeapDumpPane(new HeapDumpViewModel(dump), appManager), true) {
		}
	}
}
