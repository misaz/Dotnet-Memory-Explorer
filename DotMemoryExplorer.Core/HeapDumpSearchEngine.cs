using DotMemoryExplorer.Core;
using DotMemoryExplorer.Gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {

	public class HeapDumpSearchEngine {

		private HeapDump _dump;

		public HeapDumpSearchEngine(HeapDump dump) {
			if (dump == null) {
				throw new ArgumentNullException(nameof(dump));
			}

			_dump = dump;
		}

		public IEnumerable<SearchResult> Search(ReadOnlySpan<byte> memory) {
			List<SearchResult> output = new List<SearchResult>();

			foreach (var reg in _dump.MemoryDump.Regions) {
				output.AddRange(SearchInRegion(reg, memory));
			}

			return output;
		}

		private List<SearchResult> SearchInRegion(MemoryRegion reg, ReadOnlySpan<byte> searchFor) {
			HashSet<ulong> processedObjectsAddresses = new HashSet<ulong>();
			List<SearchResult> output = new List<SearchResult>();
			int totalOffset = 0;
			while (totalOffset < reg.Content.Length) {
				int index = reg.Content.Slice(totalOffset).IndexOf(searchFor);

				if (index == -1) {
					return output;
				}

				ulong searchResultAddress = (ulong)(totalOffset + index) + reg.Address;
				DotnetObjectMetadata metadata = new DotnetObjectMetadata();
				if (_dump.GetObjectByInnerAddress(ref metadata, searchResultAddress)) {
					if (!processedObjectsAddresses.Contains(metadata.Address)) {
						processedObjectsAddresses.Add(metadata.Address);
						output.Add(new SearchResultAddressInObject(searchResultAddress, metadata, _dump));
					}
				} else {
					output.Add(new SearchResultAddress(searchResultAddress));
				}

				totalOffset += index + 1;
			}
			return output;
		}

		public IEnumerable<SearchResult> SearchString(SearchStringRequest request) {
			return Search(request.Encoding.GetBytes(request.SearchTerm));
		}
	}

}
