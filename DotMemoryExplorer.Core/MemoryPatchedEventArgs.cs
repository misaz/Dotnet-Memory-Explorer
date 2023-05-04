namespace DotMemoryExplorer.Core {
	public class MemoryPatchedEventArgs {

		public MemoryAddressRange AddressRange { get; }

		public MemoryPatchedEventArgs(ulong address, ulong size) {
			AddressRange = new MemoryAddressRange(address, size, UnknownMemoryType.Instance, UnknownMemoryState.Instance, UnknownMemoryPermissions.Instance);
		}

		private class UnknownMemoryType: IMemoryType {

			public static UnknownMemoryType Instance = new UnknownMemoryType();

			private UnknownMemoryType() { }

		}

		private class UnknownMemoryState : IMemoryState {

			public static UnknownMemoryState Instance = new UnknownMemoryState();

			private UnknownMemoryState() { }

		}

		private class UnknownMemoryPermissions : IMemoryPermissions {

			public static UnknownMemoryPermissions Instance = new UnknownMemoryPermissions();

			private UnknownMemoryPermissions() { }

		}
	}
}