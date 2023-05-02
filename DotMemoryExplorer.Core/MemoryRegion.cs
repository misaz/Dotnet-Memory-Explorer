using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {
	public class MemoryRegion {

		private ulong _address;
		private byte[] _content;

		public ReadOnlySpan<byte> Content {
			get {
				return new ReadOnlySpan<byte>(_content);
			}
		}

		public ulong Size {
			get {
				return (ulong)_content.LongLength;
			}
		}

		public ulong Address {
			get {
				return _address;
			}
		}

		public ulong AddressEnd {
			get {
				return Address + Size;
			}
		}

		public MemoryRegion(ulong address, byte[] content) {
			if (content == null) {
				throw new ArgumentNullException(nameof(content));
			}

			_address = address;
			_content = content;
		}

		public ReadOnlySpan<byte> GetMemory(ulong absoluteAddress, ulong length) {
			if (absoluteAddress < _address || absoluteAddress >= AddressEnd) {
				throw new IndexOutOfRangeException(nameof(absoluteAddress));
			}

			if (length > int.MaxValue) {
				throw new Exception("Unable to provide memory because requested memory length is too big.");
			}

			if (absoluteAddress + length < 0) {
				throw new ArgumentException("Memory block has not enogh memory to satisfy memory request.");
			}

			ulong offset = absoluteAddress - _address;

			if (offset > int.MaxValue) {
				throw new Exception("Unable to provide memory because offset in memory is too high.");
			}

			return Content.Slice((int)offset, (int)length);
		}

		public ReadOnlySpan<byte> GetMemory(ulong absoluteAddress) {
			if (absoluteAddress < _address || absoluteAddress >= AddressEnd) {
				throw new IndexOutOfRangeException(nameof(absoluteAddress));
			}

			ulong offset = absoluteAddress - Address;

			if (offset > int.MaxValue) {
				throw new Exception("Unable to provide memory area due to underlaying technology limitation.");
			}

			return Content.Slice((int)offset);
		}

		/// <summary>
		/// Checks that region overlaps with other region. Regions overlap if there exist at least on byte of memory 
		/// which is avalaible in both regions or when both regions have zero length and both have the same address.
		/// </summary>
		/// <param name="otherRegion">Second region to check overlap with.</param>
		/// <returns>True if overlap exists. Otherwise false.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public bool OverlapWith(MemoryRegion otherRegion) {
			if (otherRegion == null) {
				throw new ArgumentNullException(nameof(otherRegion));
			}

			if (otherRegion == this) {
				return true;
			}

			if (Size == 0 || otherRegion.Size == 0) {
				return false;
			}

			if (ContainsAddres(otherRegion.Address) || ContainsAddres(otherRegion.AddressEnd - 1)) {
				return true;
			}

			if (otherRegion.Address <= this.Address && otherRegion.AddressEnd >= this.AddressEnd) {
				return true;
			}

			return false;
		}

		/// <summary>
		/// Checks that specified memory address is included inside this region.
		/// </summary>
		/// <param name="address">Address to check existence in</param>
		/// <returns>True if memory at this address is included in this region. False otherwise.</returns>
		public bool ContainsAddres(ulong address) {
			return address >= Address && address < AddressEnd;
		}
	}
}
