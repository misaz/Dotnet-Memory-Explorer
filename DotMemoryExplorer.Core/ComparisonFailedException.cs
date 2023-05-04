using System.Runtime.Serialization;

namespace DotMemoryExplorer.Core {
	[Serializable]
	public class ComparisonFailedException : Exception {
		public ComparisonFailedException() {
		}

		public ComparisonFailedException(string? message) : base(message) {
		}

		public ComparisonFailedException(string? message, Exception? innerException) : base(message, innerException) {
		}

		protected ComparisonFailedException(SerializationInfo info, StreamingContext context) : base(info, context) {
		}
	}
}