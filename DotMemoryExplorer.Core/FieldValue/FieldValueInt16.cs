namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueInt16 : FieldContent, IEditableFieldValue {
		public Int16 Value { get; }

		public FieldValueInt16(Int16 value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			Int16 dummy;
			return Int16.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return BitConverter.GetBytes(Int16.Parse(valueString));
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}