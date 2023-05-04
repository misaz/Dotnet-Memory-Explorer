namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValuePointer : FieldContent {
		public ulong Value { get; }

		public FieldValuePointer(ulong value) {
			this.Value = value;
		}
	}
}