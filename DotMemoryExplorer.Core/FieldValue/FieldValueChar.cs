namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueChar : FieldContent {
		private char Value { get; }

		public FieldValueChar(char value) {
			this.Value = value;
		}
		public override string ToString() {
			return Value.ToString();
		}
	}
}