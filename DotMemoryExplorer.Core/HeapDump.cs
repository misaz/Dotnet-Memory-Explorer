using DotMemoryExplorer.Gui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {
	public class HeapDump {

		private List<ulong> _sortedKnownAddresses;
		private SortedDictionary<ulong, DotnetObjectMetadata> _addressToObjects;
		private SortedDictionary<ulong, DotnetTypeMetadata> _typeIdToType;

		public MemoryDump MemoryDump { get; }

		public IEnumerable<DotnetObjectMetadata> Objects {
			get {
				return _addressToObjects.Values;
			}
		}

		public IEnumerable<DotnetTypeMetadata> Types {
			get {
				return _typeIdToType.Values;
			}
		}

		public DataTypeObjectGrouping DataTypeObjectGrouping { get; }

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

		public HeapDumpSearchEngine SearchEngine { get; }

		public HeapDump(MemoryDump memory, SortedDictionary<ulong, DotnetObjectMetadata> addressToObjects, SortedDictionary<ulong, DotnetTypeMetadata> typeIdToType, IDotnetProcess owningProcess) {
			if (memory == null) {
				throw new ArgumentNullException(nameof(memory));
			}

			if (addressToObjects == null) {
				throw new ArgumentNullException(nameof(addressToObjects));
			}

			if (typeIdToType == null) {
				throw new ArgumentNullException(nameof(typeIdToType));
			}

			if (owningProcess == null) {
				throw new ArgumentNullException(nameof(owningProcess));
			}

			_addressToObjects = addressToObjects;
			_typeIdToType = typeIdToType;
			_sortedKnownAddresses = new List<ulong>(addressToObjects.Keys);

			MemoryDump = memory;
			OwningProcess = owningProcess;
			DataTypeObjectGrouping = new DataTypeObjectGrouping(_addressToObjects, _typeIdToType);
			SearchEngine = new HeapDumpSearchEngine(this);
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

		public DotnetTypeMetadata GetTypeById(ulong typeId) {
			if (_typeIdToType.ContainsKey(typeId)) {
				return _typeIdToType[typeId];
			} else {
				return new DotnetTypeMetadata(typeId);
			}
		}

		public DotnetObjectMetadata GetObjectByAddress(ulong objAddr) {
			if (_addressToObjects.ContainsKey(objAddr)) {
				return _addressToObjects[objAddr];
			} else {
				throw new ArgumentException("There is no such object.");
			}
		}

		internal bool GetObjectByInnerAddress(ref DotnetObjectMetadata metadata, ulong addr) {
			int index = _sortedKnownAddresses.BinarySearch(addr);

			if (index >= 0) {
				metadata = GetObjectByAddress(addr);
			} else {
				int bitFlipped = ~index - 1;
				if (bitFlipped != -1) {
					metadata = GetObjectByAddress(_sortedKnownAddresses[bitFlipped]);
				} else {
					return false;
				}

			}

			if (addr >= metadata.Address + metadata.Size) {
				return false;
			}

			return true;
		}

		public void PatchMemory(ulong compareAddress, ReadOnlySpan<byte> compareMemory, ulong writeAddress, ReadOnlySpan<byte> writeMemory) {
			OwningProcess.ProcessMemoryManger.PatchMemory(compareAddress, compareMemory, writeAddress, writeMemory);
			MemoryDump.PatchMemory(writeAddress, writeMemory);
		}
	}
}
