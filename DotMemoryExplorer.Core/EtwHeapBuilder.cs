using Microsoft.Diagnostics.Tracing.Parsers.Clr;

namespace DotMemoryExplorer.Core {
	public class EtwHeapBuilder {
		private readonly List<GCBulkTypeTraceData> _bulkTypes;
		private readonly List<GCBulkNodeTraceData> _bulkNodes;
		private readonly MemoryDump _memoryDump;
		private readonly IDotnetProcess _owningProcess;
		private readonly EdgesProcessingQueue _edgesQueue;
		private bool _isBuilt = false;

		private SortedDictionary<ulong, DotnetObjectMetadata> _addressToObject = new SortedDictionary<ulong, DotnetObjectMetadata>();
		private SortedDictionary<ulong, DotnetTypeMetadata> _typeIdToType = new SortedDictionary<ulong, DotnetTypeMetadata>();

		public int ObjectsCount {
			get {
				return _addressToObject.Count;
			}
		}

		public int ReferencesCount {
			get {
				if (!_isBuilt) {
					throw new InvalidOperationException("References count is not computed yet. Build the heap first.");
				}
				return _edgesQueue.ProcessedReferencesCount;
			}
		}

		public EtwHeapBuilder(List<GCBulkTypeTraceData> bulkTypes, List<GCBulkNodeTraceData> bulkNodes, Queue<GCBulkEdgeTraceData> bulkEdges, MemoryDump memoryDump, IDotnetProcess owningProcess) {
			if (bulkTypes == null) {
				throw new ArgumentNullException(nameof(bulkTypes));
			}

			if (bulkNodes == null) {
				throw new ArgumentNullException(nameof(bulkNodes));
			}

			if (bulkEdges == null) {
				throw new ArgumentNullException(nameof(bulkEdges));
			}

			if (memoryDump == null) {
				throw new ArgumentNullException(nameof(memoryDump));
			}

			_bulkTypes = bulkTypes;
			_bulkNodes = bulkNodes;
			_memoryDump = memoryDump;
			this._owningProcess = owningProcess;
			_edgesQueue = new EdgesProcessingQueue(bulkEdges);
		}

		public HeapDump Build() {
			ProcessBulkNodes();
			ProcessBulkTypes();
			BuildBackReference();
			_isBuilt = true;

			return new HeapDump(_memoryDump, _addressToObject, _typeIdToType, _owningProcess);
		}

		private void BuildBackReference() {
			foreach (var obj in _addressToObject.Values) {
				foreach (var reference in obj.References) {
					_addressToObject[reference.TargetObjectAddress].ReferencedBy.Add(new DotnetReferenceMetadata(-1, obj.Address));
				}
			}
		}

		private unsafe void ProcessBulkNodes() {
			foreach (var bulkNode in _bulkNodes) {
				for (int i = 0; i < bulkNode.Count; i++) {
					GCBulkNodeUnsafeNodes node = new GCBulkNodeUnsafeNodes();
					GCBulkNodeUnsafeNodes* obj = bulkNode.UnsafeNodes(i, &node);

					ProcessNode(obj);
				}
			}
		}

		private unsafe void ProcessNode(GCBulkNodeUnsafeNodes* node) {
			DotnetReferenceMetadata[] objectReferences = new DotnetReferenceMetadata[node->EdgeCount];
			for (int j = 0; j < node->EdgeCount; j++) {
				objectReferences[j] = _edgesQueue.GetNextReference();
			}

			var inst = new DotnetObjectMetadata(node->TypeID, node->Address, node->Size, objectReferences);

			_addressToObject.Add(inst.Address, inst);
		}

		private void ProcessBulkTypes() {
			foreach (var bulkType in _bulkTypes) {
				for (int i = 0; i < bulkType.Count; i++) {
					ProcessType(bulkType.Values(i));
				}
			}
		}

		private void ProcessType(GCBulkTypeValues type) {
			var ti = new DotnetTypeMetadata(type.TypeID, type.TypeName);
			if (!_typeIdToType.ContainsKey(type.TypeID)) {
				_typeIdToType.Add(type.TypeID, ti);
			}
		}
	}
}
