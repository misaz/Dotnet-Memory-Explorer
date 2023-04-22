namespace DotMemoryExplorer.Core {
	public interface IHeapDumper : IDisposable {
		public HeapDump MakeDump();
	}
}