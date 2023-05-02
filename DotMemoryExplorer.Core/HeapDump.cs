using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {
	public class HeapDump {

		public MemoryDump MemoryDump { get; }

		public IEnumerable<DotnetObjectMetadata> Objects { get; }
		public IEnumerable<DotnetTypeMetadata> Types { get; }
		public IDotnetProcess OwningProcess { get; }

		// TODO throw if accessed when _areTimeMarksSet == false
		public DateTime CreationStarted { get; private set; }
		public DateTime CreationCompleted { get; private set; }
		public DateTime ProcessingCompleted { get; private set; }
		private bool _areTimeMarksSet = false;

		// TODO throw if accessed when _areStatisticsSet == false
		public int ObjectsCount { get; private set; }
		public int ReferencesCount { get; private set; }
		private bool _areStatisticsSet = false;

		public HeapDump(MemoryDump memory, IEnumerable<DotnetObjectMetadata> objects, IEnumerable<DotnetTypeMetadata> types, IDotnetProcess owningProcess) {
			MemoryDump = memory;
			Objects = objects;
			Types = types;
			OwningProcess = owningProcess;
		}

		public void SetTimeMarks(DateTime creationStarted, DateTime creationCompleted, DateTime processingCompleted) {
			if (_areTimeMarksSet) {
				throw new InvalidOperationException("Time marks are already set");
			}

			if (creationStarted > creationCompleted) {
				throw new InvalidEnumArgumentException("Creation start datetime must happen before completion.");
			}

			if (creationCompleted > processingCompleted) {
				throw new InvalidEnumArgumentException("Processing must complete after heap dump collection.");
			}

			CreationStarted = creationStarted;
			CreationCompleted = creationCompleted;
			ProcessingCompleted = processingCompleted;
			_areTimeMarksSet = true;
		}

		public void SetStatistics(int objectsCount, int referencesCount) {
			if (_areStatisticsSet) {
				throw new InvalidOperationException("Statistics are already set");
			}

			_areStatisticsSet = true;

			ObjectsCount = objectsCount;
			ReferencesCount = referencesCount;
		}
	}
}
