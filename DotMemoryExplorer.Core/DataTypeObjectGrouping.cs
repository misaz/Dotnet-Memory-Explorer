using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Gui {

	public class DataTypeObjectGrouping {

		private SortedDictionary<ulong, List<DotnetObjectMetadata>> _dataTypeToObjects = new SortedDictionary<ulong, List<DotnetObjectMetadata>>();

		private List<DataTypeStatisticsEntry> _stats;

		public IEnumerable<DataTypeStatisticsEntry> Statistics {
			get {
				return _stats.AsReadOnly();
			}
		}

		public DataTypeObjectGrouping(SortedDictionary<ulong, DotnetObjectMetadata> _addressToObjects, SortedDictionary<ulong, DotnetTypeMetadata> _typeIdToTypes) {
			foreach (var addrToObj in _addressToObjects) {
				DotnetObjectMetadata obj = addrToObj.Value;

				if (_dataTypeToObjects.ContainsKey(obj.TypeId)) {
					_dataTypeToObjects[obj.TypeId].Add(obj);
				} else {
					_dataTypeToObjects.Add(obj.TypeId, new List<DotnetObjectMetadata> { obj });
				}
			}

			_stats = new List<DataTypeStatisticsEntry>(_dataTypeToObjects.Count);

			foreach (var entry in _dataTypeToObjects) {
				DotnetTypeMetadata type;
				if (_typeIdToTypes.ContainsKey(entry.Key)) {
					type = _typeIdToTypes[entry.Key];
				} else {
					type = new DotnetTypeMetadata(entry.Key, $"0x{entry.Key.ToString("X16")}");
				}
				_stats.Add(new DataTypeStatisticsEntry(type, entry.Value.Count));
			}
		}

		public IEnumerable<DotnetObjectMetadata> GetObjectsByTypeId(ulong typeId) {
			if (!_dataTypeToObjects.ContainsKey(typeId)) {
				return Enumerable.Empty<DotnetObjectMetadata>();
			} else {
				List<DotnetObjectMetadata> objectsList = _dataTypeToObjects[typeId];
				return objectsList.AsReadOnly();
			}
		}
	}
}
