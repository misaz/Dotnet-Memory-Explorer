using DotMemoryExplorer.Core;

namespace DotMemoryExplorer.Core {
	public interface IEditableFieldValue {

		public bool IsValid(string valueString);
		public byte[] GetValueBytes(string valueString);
		public string BuildValueString();

	}
}