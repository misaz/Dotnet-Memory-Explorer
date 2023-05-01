namespace DotMemoryExplorer.Core {
	public interface IProcessMemoryManger {
		IEnumerable<MemoryAddressRange> GetMemoryRegions();
		MemoryDump MakeMemoryDump();
	}
}