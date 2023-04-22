namespace DotMemoryExplorer.Core {

	public class EtwHeapDumperFactory : IHeapDumperFactory {

		private LiveDotnetProcess _process;

		public EtwHeapDumperFactory(LiveDotnetProcess process) {
			_process = process;
		}

		public IHeapDumper CreateDumper() {
			return new EtwHeapDumper(_process);
		}

	}
}