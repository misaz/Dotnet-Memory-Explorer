namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueNativeUInt : FieldContent, IEditableFieldValue {
		public ulong Value { get; }
		public ulong PointerSize { get; }

		public FieldValueNativeUInt(ulong value, ulong pointerSize) {
			this.Value = value;
			PointerSize = pointerSize;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			if (PointerSize == 8) {
				ulong dummy;
				return ulong.TryParse(valueString, out dummy);
			} else {
				uint dummy;
				return uint.TryParse(valueString, out dummy);
			}
		}

		public byte[] GetValueBytes(string valueString) {
			if (PointerSize == 8) {
				return BitConverter.GetBytes(ulong.Parse(valueString));
			} else {
				return BitConverter.GetBytes(uint.Parse(valueString));
			}
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}