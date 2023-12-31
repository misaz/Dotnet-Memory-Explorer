﻿using Microsoft.Diagnostics.Tracing.Parsers.FrameworkEventSource;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftAntimalwareAMFilter;
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

		private readonly Process _proc;

		public WindowsProcessMemoryManger(Process proc) {
			if (proc == null) {
				throw new ArgumentNullException(nameof(proc));
			}

			_proc = proc;
		}

		public unsafe ReadOnlySpan<byte> GetMemory(ulong address, ulong size) {
			AssureUnmanagedBufferCapacity((nuint)size);

			nuint read;
			PInvoke.ReadProcessMemory(_proc.SafeHandle, (void*)address, (void*)buffer, (nuint)size, &read);

			if (size > int.MaxValue) {
				throw new Exception("Unable to read memory because request size is too large.");
			}

			byte[] managedBuffer = new byte[size];
			Marshal.Copy(buffer, managedBuffer, 0, (int)size);

			return new ReadOnlySpan<byte>(managedBuffer);
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

			using (var suspendBlock = new NtSuspendBlock(_proc)) {
				IEnumerable<MemoryAddressRange> regionsAddresses = GetMemoryRegions();
				List<MemoryRegion> regions = new List<MemoryRegion>(regionsAddresses.Count());


				foreach (var region in regionsAddresses) {
					// dump only private commited memory
					if (region.Type != WindowsMemoryType.Private || region.State != WindowsMemoryState.Commit) {
						continue;
					}

					AssureUnmanagedBufferCapacity((nuint)region.Size);

					nuint read;
					PInvoke.ReadProcessMemory(p, (void*)region.Address, (void*)buffer, (nuint)region.Size, &read);

					byte[] managedBuffer = new byte[region.Size];
					Marshal.Copy(buffer, managedBuffer, 0, (int)region.Size);

					regions.Add(new MemoryRegion((ulong)region.Address, managedBuffer));
				}

				return new MemoryDump(regions);
			}
		}


		private IntPtr buffer = IntPtr.Zero;
		private nuint bufferSize = 0;

		private void AssureUnmanagedBufferCapacity(nuint requestedCapacity) {
			if (requestedCapacity > bufferSize) {
				// free old insufficient buffer
				if (buffer != 0) {
					Marshal.FreeHGlobal(buffer);
				}

				if (requestedCapacity > int.MaxValue) {
					throw new Exception($"Unable to make memry dump because some region is larger than {int.MaxValue} bytes.");
				}

				buffer = Marshal.AllocHGlobal((int)requestedCapacity);
				bufferSize = (nuint)requestedCapacity;
			}
		}

		public void Dispose() {
			if (buffer != 0) {
				Marshal.FreeHGlobal(buffer);
				buffer = 0;
				bufferSize = 0;
			}
		}

		public unsafe void PatchMemory(ulong compareAddress, ReadOnlySpan<byte> compareMemory, ulong writeAddress, ReadOnlySpan<byte> writeMemory) {
			Windows.Win32.Foundation.BOOL status;
			SafeHandle p = _proc.SafeHandle;

			using (var suspendBlock = new NtSuspendBlock(_proc)) {
				AssureUnmanagedBufferCapacity((nuint)compareMemory.Length);

				nuint read;
				status = PInvoke.ReadProcessMemory(p, (void*)compareAddress, (void*)buffer, (nuint)compareMemory.Length, &read);
				if (status == false || read != (nuint)compareMemory.Length) {
					throw new Exception("ReadProcessMemory failed to read requested amount of memory.");
				}
				byte[] managedBuffer = new byte[compareMemory.Length];
				Marshal.Copy(buffer, managedBuffer, 0, (int)compareMemory.Length);

				if (!compareMemory.SequenceEqual(new ReadOnlySpan<byte>(managedBuffer))) {
					throw new ComparisonFailedException();
					throw new Exception("Unable to patch memory because object changed it's location or content since dump was collected");
				}

				AssureUnmanagedBufferCapacity((nuint)writeMemory.Length);

				Marshal.Copy(writeMemory.ToArray(), 0, buffer, writeMemory.Length);

				nuint written;
				status = PInvoke.WriteProcessMemory(p, (void*)writeAddress, (void*)buffer, (nuint)writeMemory.Length, &written);
				if (status == false || written != (nuint)writeMemory.Length) {
					throw new Exception("WriteProcessMemory failed to write requested amount of memory.");
				}
			}
		}
	}
}
