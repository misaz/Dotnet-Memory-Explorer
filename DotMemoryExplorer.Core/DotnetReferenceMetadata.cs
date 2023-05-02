namespace DotMemoryExplorer.Core {
	public struct DotnetReferenceMetadata {

		private int _referencingFieldId;
		private ulong _target;

		public DotnetReferenceMetadata(int referencingFieldId, ulong target) {
			_referencingFieldId = referencingFieldId;
			_target = target;
		}
	}
}