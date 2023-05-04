namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueInt64 : FieldContent, IEditableFieldValue {
		public Int64 Value { get; }

		public FieldValueInt64(Int64 value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			Int64 dummy;
			return Int64.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return BitConverter.GetBytes(Int64.Parse(valueString));
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}