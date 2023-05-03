namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueInt32 : FieldContent {
		public int Value { get; }

		public FieldValueInt32(int value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}
	}
}