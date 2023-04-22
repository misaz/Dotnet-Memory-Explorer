namespace DotMemoryExplorer.Core {

	internal class EtwHeapDumper : IHeapDumper {

		private readonly LiveDotnetProcess _liveDotnetProcess;

		public EtwHeapDumper(LiveDotnetProcess liveDotnetProcess) {
			if (liveDotnetProcess == null) {
				throw new ArgumentNullException(nameof(liveDotnetProcess));
			}

			_liveDotnetProcess = liveDotnetProcess;
		}

		public HeapDump MakeDump() {
			throw new NotImplementedException();
		}

		public void Dispose() {
		}
	}

}