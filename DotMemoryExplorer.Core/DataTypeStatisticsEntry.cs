using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {
	public struct DataTypeStatisticsEntry {
		public DotnetTypeMetadata Type { get; }
		public int ObjectsCount { get; }

		public DataTypeStatisticsEntry(DotnetTypeMetadata type, int objectsCount) {
			Type = type;
			ObjectsCount = objectsCount;
		}
	}
}
