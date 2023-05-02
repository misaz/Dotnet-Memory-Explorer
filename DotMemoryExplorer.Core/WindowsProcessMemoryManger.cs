using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.System.Memory;

namespace DotMemoryExplorer.Core {
	public class WindowsProcessMemoryManger : IMemoryDumper, IProcessMemoryManger {

		public Process _proc;

		public WindowsProcessMemoryManger(Process proc) {
			if (proc == null) {
				throw new ArgumentNullException(nameof(proc));
			}

			_proc = proc;
		}

		/// <summary>
		/// Return informations about memory pages alocated by owned process.
		/// </summary>
		/// <returns>Enumerable of memory regions</returns>
		/// <exception cref="Exception"></exception>
		public unsafe IEnumerable<MemoryAddressRange> GetMemoryRegions() {
			UIntPtr offset = UIntPtr.Zero;
			SafeHandle p = _proc.SafeHandle;
			MEMORY_BASIC_INFORMATION basicInfo;
			nuint basicInfoSize = (nuint)Marshal.SizeOf<MEMORY_BASIC_INFORMATION>();

			List<MemoryAddressRange> regions = new List<MemoryAddressRange>();

			nuint retval;
			do {
				retval = PInvoke.VirtualQueryEx(p, (void*)offset, out basicInfo, basicInfoSize);
				if (retval == 0) {
					int error = Marshal.GetLastWin32Error();
					if (error == 87) { //87 is ERROR_INVALID_PARAMETER which is triggered when enumerating after last avalaible page
						break;
					}

					throw new Exception($"Remote process memory enumeration failed with status code {error}");
				}

				offset += basicInfo.RegionSize;

				if (basicInfo.State == VIRTUAL_ALLOCATION_TYPE.MEM_FREE) {
					continue;
				}

				WindowsMemoryType type = WindowsMemoryType.GetCached((uint)basicInfo.Type);
				WindowsMemoryState state = WindowsMemoryState.GetCached(basicInfo.State);
				WindowsMemoryPermissions perms = WindowsMemoryPermissions.GetCached((uint)basicInfo.Protect);

				regions.Add(new MemoryAddressRange((ulong)basicInfo.BaseAddress, (ulong)basicInfo.RegionSize, type, state, perms));

			} while (retval != 0);

			return regions.AsReadOnly();
		}

		public unsafe MemoryDump MakeMemoryDump() {
			SafeHandle p = _proc.SafeHandle;

			IEnumerable<MemoryAddressRange> regionsAddresses = GetMemoryRegions();
			List<MemoryRegion> regions = new List<MemoryRegion>(regionsAddresses.Count());

			IntPtr buffer = IntPtr.Zero;
			nuint bufferSize = 0;

			foreach (var region in regionsAddresses) {
				// dump only private commited memory
				if (region.Type != WindowsMemoryType.Private || region.State != WindowsMemoryState.Commit) {
					continue;
				}

				if (region.Size > bufferSize) {
					if (buffer != 0) {
						Marshal.FreeHGlobal(buffer);
					}

					if (region.Size > int.MaxValue) {
						throw new Exception($"Unable to make memry dump because some region is larger than {int.MaxValue} bytes.");
					}

					buffer = Marshal.AllocHGlobal((int)region.Size);
					bufferSize = (nuint)region.Size;
				}

				nuint read;
				PInvoke.ReadProcessMemory(p, (void*)region.Address, (void*)buffer, (nuint)region.Size, &read);

				byte[] managedBuffer = new byte[region.Size];
				Marshal.Copy(buffer, managedBuffer, 0, (int)region.Size);

				regions.Add(new MemoryRegion((ulong)region.Address, managedBuffer));
			}

			return new MemoryDump(regions);
		}

	}
}
