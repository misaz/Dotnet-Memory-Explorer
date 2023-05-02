namespace DotMemoryExplorer.Core {
	public interface IProcessMemoryManger : IDisposable {
		ReadOnlySpan<byte> GetMemory(ulong address, ulong size);
		IEnumerable<MemoryAddressRange> GetMemoryRegions();
		MemoryDump MakeMemoryDump();
	}
}