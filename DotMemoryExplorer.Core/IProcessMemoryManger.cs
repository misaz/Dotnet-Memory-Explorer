namespace DotMemoryExplorer.Core {
	public interface IProcessMemoryManger : IDisposable {
		ReadOnlySpan<byte> GetMemory(ulong address, ulong size);
		IEnumerable<MemoryAddressRange> GetMemoryRegions();
		MemoryDump MakeMemoryDump();
		void PatchMemory(ulong compareAddress, ReadOnlySpan<byte> compareMemory, ulong writeAddress, ReadOnlySpan<byte> writeMemory);
	}
}