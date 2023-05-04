namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueDouble : FieldContent, IEditableFieldValue {
		private double Value { get; }

		public FieldValueDouble(double value) {
			this.Value = value;
		}
		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			double dummy;
			return double.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return BitConverter.GetBytes(double.Parse(valueString));
		}

		public string BuildValueString() {
			return ToString();
		}
	}
}