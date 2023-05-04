namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueUInt16 : FieldContent, IEditableFieldValue {
		public UInt16 Value { get; }

		public FieldValueUInt16(UInt16 value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			UInt16 dummy;
			return UInt16.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return BitConverter.GetBytes(UInt16.Parse(valueString));
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}