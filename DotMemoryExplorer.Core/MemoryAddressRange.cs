using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {

	public class MemoryAddressRange {

		private readonly ulong _address;
		private readonly ulong _size;
		private readonly IMemoryType _type;
		private readonly IMemoryState _state;
		private readonly IMemoryPermissions _permissions;

		public ulong Size {
			get {
				return _size;
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

		public IMemoryType Type {
			get {
				return _type;
			}
		}

		public IMemoryState State {
			get {
				return _state;
			}
		}

		public IMemoryPermissions Permissions {
			get {
				return _permissions;
			}
		}

		public MemoryAddressRange(ulong address, ulong size, IMemoryType type, IMemoryState state, IMemoryPermissions permissions) {
			if (type is null) {
				throw new ArgumentNullException(nameof(type));
			}

			if (state is null) {
				throw new ArgumentNullException(nameof(state));
			}

			if (permissions is null) {
				throw new ArgumentNullException(nameof(permissions));
			}

			_address = address;
			_size = size;
			_type = type;
			_state = state;
			_permissions = permissions;
		}

	}
}
