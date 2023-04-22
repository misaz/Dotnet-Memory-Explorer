using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {
	public class HeapDump {

		public MemoryDump MemoryDump { get; }

		public IEnumerable<DotnetObjectMetadata> Objects { get; }

		public HeapDump(MemoryDump memory, IEnumerable<DotnetObjectMetadata> objects) {
			MemoryDump = memory;
			Objects = objects;
		}

	}
}
