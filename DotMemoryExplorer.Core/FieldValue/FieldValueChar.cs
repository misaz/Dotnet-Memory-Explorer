namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueChar : FieldContent {
		private char Value { get; }

		public FieldValueChar(char value) {
			this.Value = value;
		}
		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			char dummy;
			return char.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return BitConverter.GetBytes(char.Parse(valueString));
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}