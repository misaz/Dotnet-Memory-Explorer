using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.System.Memory;

namespace DotMemoryExplorer.Core {
	public class WindowsMemoryState : IMemoryState {

		public static readonly WindowsMemoryState Commit = new WindowsMemoryState(VIRTUAL_ALLOCATION_TYPE.MEM_COMMIT, "Commit");
		public static readonly WindowsMemoryState Free = new WindowsMemoryState(VIRTUAL_ALLOCATION_TYPE.MEM_FREE, "Free");
		public static readonly WindowsMemoryState Reserve = new WindowsMemoryState(VIRTUAL_ALLOCATION_TYPE.MEM_RESERVE, "Reserve");
		public static readonly WindowsMemoryState ReservePlaceholder = new WindowsMemoryState(VIRTUAL_ALLOCATION_TYPE.MEM_RESERVE_PLACEHOLDER, "Reserve Placeholder");
		public static readonly WindowsMemoryState ReplacePlaceholder = new WindowsMemoryState(VIRTUAL_ALLOCATION_TYPE.MEM_REPLACE_PLACEHOLDER, "Replace Placeholder");
		public static readonly WindowsMemoryState Reset = new WindowsMemoryState(VIRTUAL_ALLOCATION_TYPE.MEM_RESET, "Reset");
		public static readonly WindowsMemoryState ResetUndo = new WindowsMemoryState(VIRTUAL_ALLOCATION_TYPE.MEM_RESET_UNDO, "Reset Undo");
		public static readonly WindowsMemoryState LargePages = new WindowsMemoryState(VIRTUAL_ALLOCATION_TYPE.MEM_LARGE_PAGES, "Large Pages");

		private VIRTUAL_ALLOCATION_TYPE _type;
		private string _name;

		private WindowsMemoryState(VIRTUAL_ALLOCATION_TYPE type, string name) {
			_type = type;
			_name = name;
		}

		public override string ToString() {
			return _name;
		}

		private static Dictionary<VIRTUAL_ALLOCATION_TYPE, WindowsMemoryState> _cachedStates = new Dictionary<VIRTUAL_ALLOCATION_TYPE, WindowsMemoryState> {
			{ VIRTUAL_ALLOCATION_TYPE.MEM_COMMIT, Commit },
			{ VIRTUAL_ALLOCATION_TYPE.MEM_FREE, Free },
			{ VIRTUAL_ALLOCATION_TYPE.MEM_RESERVE, Reserve },
			{ VIRTUAL_ALLOCATION_TYPE.MEM_RESERVE_PLACEHOLDER, ReservePlaceholder },
			{ VIRTUAL_ALLOCATION_TYPE.MEM_REPLACE_PLACEHOLDER, ReplacePlaceholder },
			{ VIRTUAL_ALLOCATION_TYPE.MEM_RESET, Reset },
			{ VIRTUAL_ALLOCATION_TYPE.MEM_RESET_UNDO, ResetUndo },
			{ VIRTUAL_ALLOCATION_TYPE.MEM_LARGE_PAGES, LargePages },
		};

		internal static WindowsMemoryState GetCached(VIRTUAL_ALLOCATION_TYPE state) {
			if (_cachedStates.ContainsKey(state)) {
				return _cachedStates[state];
			} else {
				WindowsMemoryState inst = new WindowsMemoryState(state, $"State 0x{((uint)state).ToString("X")}");
				_cachedStates.Add(state, inst);
				return inst;
			}
		}

	}
}
