namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueInt8 : FieldContent {
		public sbyte Value { get; }

		public FieldValueInt8(sbyte value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}
	}
}