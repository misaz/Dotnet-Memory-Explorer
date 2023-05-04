namespace DotMemoryExplorer.Core.FieldValue {
	public class FieldValueNativeInt : FieldContent, IEditableFieldValue {
		public long Value { get; }
		public ulong PointerSize { get; }

		public FieldValueNativeInt(long value, ulong pointerSize) {
			this.Value = value;
			PointerSize = pointerSize;
		}

		public override string ToString() {
			return Value.ToString();
		}

		public bool IsValid(string valueString) {
			if (PointerSize == 8) {
				long dummy;
				return long.TryParse(valueString, out dummy);
			} else {
				int dummy;
				return int.TryParse(valueString, out dummy);
			}
		}

		public byte[] GetValueBytes(string valueString) {
			if (PointerSize == 8) {
				return BitConverter.GetBytes(long.Parse(valueString));
			} else {
				return BitConverter.GetBytes(int.Parse(valueString));
			}
		}

		public string BuildValueString() {
			return ToString();
		}

	}
}