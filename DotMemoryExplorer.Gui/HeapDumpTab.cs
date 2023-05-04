using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	public class HeapDumpTab : Tab {
		public HeapDump HeapDump { get; }

		public HeapDumpTab(int dumpNumber, HeapDump heapDump, ApplicationManager appManager) : base($"Dump #{dumpNumber} of {heapDump.OwningProcess.ToString()}", new HeapDumpPane(heapDump, appManager), true) {
			if (heapDump == null) {
				throw new ArgumentNullException(nameof(heapDump));
			}

			HeapDump = heapDump;
		}

	}
}
