namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueFunctionPointer : FieldContent {
		public ulong Value { get; }

		public FieldValueFunctionPointer(ulong value) {
			this.Value = value;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			ulong dummy;
			return ulong.TryParse(valueString, out dummy);
		}

		public byte[] GetValueBytes(string valueString) {
			return BitConverter.GetBytes(ulong.Parse(valueString));
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}