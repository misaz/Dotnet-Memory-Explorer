namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueFloat : FieldContent, IEditableFieldValue {
		private float Value { get; }

		public FieldValueFloat(float value) {
			this.Value = value;
		}
		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			float dummy;
			return float.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return BitConverter.GetBytes(float.Parse(valueString));
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}