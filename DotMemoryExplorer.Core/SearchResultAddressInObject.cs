namespace DotMemoryExplorer.Core {
	public class SearchResultAddressInObject : SearchResult {
		public DotnetObjectMetadata ObejctMetadata { get; }
		public HeapDump OwningHeapDump { get; }

		public SearchResultAddressInObject(ulong occurenceAddress, DotnetObjectMetadata obejctMetadata, HeapDump owningHeapDump) : base(occurenceAddress) {
			ObejctMetadata = obejctMetadata;
			OwningHeapDump = owningHeapDump;
		}
	}
}