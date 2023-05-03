namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueBoolean : FieldContent {
		public bool Value { get; }

		public FieldValueBoolean(bool value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}
	}
}