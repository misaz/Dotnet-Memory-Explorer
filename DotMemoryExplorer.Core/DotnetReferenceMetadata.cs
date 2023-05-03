namespace DotMemoryExplorer.Core {
	public struct DotnetReferenceMetadata {

		public int ReferencingFieldId { get; }
		public ulong TargetObjectAddress { get; }

		public DotnetReferenceMetadata(int referencingFieldId, ulong target) {
			ReferencingFieldId = referencingFieldId;
			TargetObjectAddress = target;
		}
	}
}