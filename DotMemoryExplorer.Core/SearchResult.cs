namespace DotMemoryExplorer.Core {
	public class SearchResult : IComparable {
		public ulong OccurenceAddress { get; }

		public SearchResult(ulong occurenceAddress) {
			this.OccurenceAddress = occurenceAddress;
		}

		public int CompareTo(object? obj) {
			if (obj is not SearchResult) {
				throw new InvalidOperationException($"Only two {nameof(SearchResult)} are supported for comparisons.");
			}

			SearchResult sr2 = (SearchResult)obj;
			return StringComparer.InvariantCulture.Compare(this.ToString(), sr2.ToString());
		}
	}
}