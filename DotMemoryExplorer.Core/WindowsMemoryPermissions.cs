using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.System.Memory;

namespace DotMemoryExplorer.Core {
	public class WindowsMemoryPermissions : IMemoryPermissions {

		private string _name;

		private WindowsMemoryPermissions(string name) {
			_name = name;
		}

		public string Name {
			get {
				return _name;
			}
		}

		public override string ToString() {
			return _name;
		}

		private static Dictionary<PAGE_PROTECTION_FLAGS, WindowsMemoryPermissions> _cachedPermissions = new Dictionary<PAGE_PROTECTION_FLAGS, WindowsMemoryPermissions>();

		public static WindowsMemoryPermissions GetCached(uint pageProtectionFlags) {
			PAGE_PROTECTION_FLAGS pageProtectionFlagsTyped = (PAGE_PROTECTION_FLAGS)pageProtectionFlags;
			if (_cachedPermissions.ContainsKey(pageProtectionFlagsTyped)) {
				return _cachedPermissions[pageProtectionFlagsTyped];
			} else {
				var inst = new	WindowsMemoryPermissions(pageProtectionFlagsTyped.ToString());
				_cachedPermissions.Add(pageProtectionFlagsTyped, inst);
				return inst;
			}
		}
	}
}
