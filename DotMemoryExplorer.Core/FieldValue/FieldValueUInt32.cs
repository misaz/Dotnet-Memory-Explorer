namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueUInt32 : FieldContent, IEditableFieldValue {
		public UInt32 Value { get; }

		public FieldValueUInt32(UInt32 value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			UInt32 dummy;
			return UInt32.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return BitConverter.GetBytes(UInt32.Parse(valueString));
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}