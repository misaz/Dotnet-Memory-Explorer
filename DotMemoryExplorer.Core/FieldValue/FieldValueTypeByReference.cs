namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueTypeByReference : FieldContent {
		public ulong Value { get; }

		public FieldValueTypeByReference(ulong value) {
			this.Value = value;
		}
	}
}