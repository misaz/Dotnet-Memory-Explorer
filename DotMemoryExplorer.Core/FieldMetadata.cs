namespace DotMemoryExplorer.Core {
	public struct FieldMetadata {
		public FieldId FieldId { get; }
		public ulong Offset { get; }
		public ulong IsStatic { get; }
		public ulong FieldType { get; }
		public int Index { get; }
		public FieldContent? Content { get; set; }

		public FieldMetadata(FieldId fieldId, ulong offset, ulong isStatic, ulong type, int index, FieldContent? content) {
			FieldId = fieldId;
			Offset = offset;
			IsStatic = isStatic;
			FieldType = type;
			Index = index;
			Content = content;
		}

	}
}