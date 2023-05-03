namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueUint8 : FieldContent {
		public byte Value { get; }

		public FieldValueUint8(byte value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}
	}
}