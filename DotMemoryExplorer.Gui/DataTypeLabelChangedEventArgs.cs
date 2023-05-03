namespace DotMemoryExplorer.Gui {
	public class DataTypeLabelChangedEventArgs {

		public ulong DataTypeId { get; }

		public DataTypeLabelChangedEventArgs(ulong dataTypeId) {
			DataTypeId = dataTypeId;
		}
	}
}