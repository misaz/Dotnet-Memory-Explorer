using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {
	public class MemoryDump {

		private SortedDictionary<ulong, MemoryRegion> _regions = new SortedDictionary<ulong, MemoryRegion>();

		public event EventHandler<MemoryPatchedEventArgs>? MemoryPatched;

		public IEnumerable<MemoryRegion> Regions {
			get {
				return _regions.Values;
			}
		}

		public MemoryDump(IEnumerable<MemoryRegion> regions) {
			foreach (MemoryRegion region in regions) {
				if (region == null) {
					throw new ArgumentNullException("Passed memory regions cannot contain null.");
				}

				foreach (var existingRegionEntry in _regions) {
					MemoryRegion existingRegion = existingRegionEntry.Value;

					if (region.OverlapWith(existingRegion)) {
						throw new ArgumentException("Passed memory regions cannot contain overlaping regions.");
					}
				}

				_regions.Add(region.Address, region);
			}
		}

		public ReadOnlySpan<byte> GetMemory(ulong address, ulong size) {
			// TODO: implement support for multiregion access (rarely used in practise and even without
			// implementation everything in DotMemoryExpolorer should be possible without implementing it).
			// Currently accessing memory accress two consecutive regons throws exception.

			foreach(var regionItem in _regions) {
				MemoryRegion region = regionItem.Value;

				if (region.ContainsAddres(address)) {
					return region.GetMemory(address, size);
				}
			}

			throw new ArgumentOutOfRangeException("Specified memory is not avalaible in the dump.");
		}

		internal void PatchMemory(ulong writeAddress, ReadOnlySpan<byte> writeMemory) {
			foreach (var reg in Regions) {
				if (reg.ContainsAddres(writeAddress)) {
					reg.PatchMemory(writeAddress, writeMemory);
					if (MemoryPatched != null) {
						MemoryPatched(this, new MemoryPatchedEventArgs(writeAddress, (ulong)writeMemory.Length));
					}
				}
			}
		}
	}
}
