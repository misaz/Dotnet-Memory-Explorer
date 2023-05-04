namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueUInt8 : FieldContent, IEditableFieldValue {
		public byte Value { get; }

		public FieldValueUInt8(byte value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			byte dummy;
			return byte.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return new byte[1] { byte.Parse(valueString) };
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}