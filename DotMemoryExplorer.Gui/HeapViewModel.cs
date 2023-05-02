using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Gui {
	public class HeapDumpViewModel {

		private HeapDump _heap;

		public HeapDump HeapDump {
			get {
				return _heap;
			}
		}

		public HeapDumpViewModel(HeapDump heap) {
			if (heap == null) {
				throw new ArgumentNullException(nameof(heap));
			}

			_heap = heap;
		}

	}
}
