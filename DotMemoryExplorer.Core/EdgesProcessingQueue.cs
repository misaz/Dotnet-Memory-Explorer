using Microsoft.Diagnostics.Tracing.Parsers.Clr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {
	public class EdgesProcessingQueue {

		private Queue<GCBulkEdgeTraceData> _bulkEdges;
		private GCBulkEdgeTraceData? _workingEvent;
		private int _workingEventProcessedReferences = 0;
		private int _processedReferencesCount = 0;

		public int ProcessedReferencesCount {
			get {
				return _processedReferencesCount;
			}
		}

		public EdgesProcessingQueue(Queue<GCBulkEdgeTraceData> bulkEdges) {
			if (bulkEdges == null) {
				throw new ArgumentNullException(nameof(bulkEdges));
			}

			_bulkEdges = bulkEdges;
		}

		private void LoadNextBulkEdge() {
			if (_bulkEdges.Any()) {
				_workingEvent = _bulkEdges.Dequeue();
				_workingEventProcessedReferences = 0;
			} else {
				throw new Exception("There are no more edges.");
			}
		}

		public DotnetReferenceMetadata GetNextReference() {
			if (_workingEvent == null) {
				LoadNextBulkEdge();
			}

			if (_workingEvent == null) {
				// should never ahppen because this already checked in LoadNextBulkEdge.
				// this conditions is only for bypass C# non-nullability checks warning
				throw new Exception("There are no more edges.");
			}

			if (_workingEventProcessedReferences == _workingEvent.Count) {
				LoadNextBulkEdge();
			}

			GCBulkEdgeValues edge = _workingEvent.Values(_workingEventProcessedReferences++);

			_processedReferencesCount++;

			return new DotnetReferenceMetadata(edge.ReferencingField, edge.Target);
		}
	}
}
