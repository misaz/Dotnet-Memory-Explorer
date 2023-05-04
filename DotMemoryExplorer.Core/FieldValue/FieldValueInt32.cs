namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueInt32 : FieldContent, IEditableFieldValue {
		public int Value { get; }

		public FieldValueInt32(int value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			Int32 dummy;
			return Int32.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return BitConverter.GetBytes(Int32.Parse(valueString));
		}

		public string BuildValueString() {
			return ToString();
		}
	}
}