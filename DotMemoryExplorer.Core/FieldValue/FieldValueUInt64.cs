namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueUInt64 : FieldContent, IEditableFieldValue {
		public UInt64 Value { get; }

		public FieldValueUInt64(UInt64 value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			UInt64 dummy;
			return UInt64.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return BitConverter.GetBytes(UInt64.Parse(valueString));
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}