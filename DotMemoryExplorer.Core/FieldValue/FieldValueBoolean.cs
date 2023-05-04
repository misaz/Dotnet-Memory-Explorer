namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueBoolean : FieldContent, IEditableFieldValue {
		public bool Value { get; }

		public FieldValueBoolean(bool value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			bool dummy;
			return bool.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return BitConverter.GetBytes(bool.Parse(valueString));
		}

		public string BuildValueString() {
			return ToString();
		}
	}
}