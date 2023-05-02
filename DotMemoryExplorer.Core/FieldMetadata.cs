namespace DotMemoryExplorer.Core {
	public struct FieldMetadata {
		public ulong OwnerTypeId { get; }
		public ulong Mb { get; }
		public ulong Offset { get; }
		public ulong IsStatic { get; }
		public ulong FieldType { get; }
		public int Index { get; }
		public FieldContent Content { get; }

		public FieldMetadata(ulong owningTypeId, ulong mb, ulong offset, ulong isStatic, ulong type, int index, FieldContent content) {
			OwnerTypeId = owningTypeId;
			Mb = mb;
			Offset = offset;
			IsStatic = isStatic;
			FieldType = type;
			Index = index;
			Content = content;
		}

	}
}