namespace DotMemoryExplorer.Core {
	public class DotnetTypeMetadata {
		private ulong typeID;
		private string typeName;

		public DotnetTypeMetadata(ulong typeID, string typeName) {
			this.typeID = typeID;
			this.typeName = typeName;
		}
	}
}