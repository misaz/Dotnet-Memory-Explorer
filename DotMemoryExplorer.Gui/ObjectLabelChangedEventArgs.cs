namespace DotMemoryExplorer.Gui {
	public class ObjectLabelChangedEventArgs {

		public ulong ObjectAddress { get; }

		public ObjectLabelChangedEventArgs(ulong objectId) {
			ObjectAddress = objectId;
		}
	}
}