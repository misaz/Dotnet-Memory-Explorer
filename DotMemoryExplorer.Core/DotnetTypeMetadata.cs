namespace DotMemoryExplorer.Core {
	public struct DotnetTypeMetadata {
		public ulong TypeId { get; }
		public string TypeName { get; }

		public DotnetTypeMetadata(ulong typeId, string typeName) {
			this.TypeId = typeId;
			this.TypeName = typeName;
		}

		public DotnetTypeMetadata(ulong typeId) {
			this.TypeId = typeId;
			this.TypeName = $"0x{typeId.ToString("16X")}";
		}

	}
}