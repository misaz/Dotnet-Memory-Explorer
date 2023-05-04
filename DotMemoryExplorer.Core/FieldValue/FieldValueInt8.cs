namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueInt8 : FieldContent, IEditableFieldValue {
		public sbyte Value { get; }

		public FieldValueInt8(sbyte value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			sbyte dummy;
			return sbyte.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return new byte[1] { (unchecked((byte)sbyte.Parse(valueString))) };
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}