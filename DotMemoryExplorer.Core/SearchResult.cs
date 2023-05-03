namespace DotMemoryExplorer.Core {
	public class SearchResult {
		public ulong OccurenceAddress { get; }

		public SearchResult(ulong occurenceAddress) {
			this.OccurenceAddress = occurenceAddress;
		}

	}
}