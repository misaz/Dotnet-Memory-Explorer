using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {
	public class StandardMemoryPermissions : IMemoryPermissions {

		private bool _canRead;
		private bool _canWrite;
		private bool _canExecute;
		private string _cachedPermissionsString;

		public bool CanRead {
			get {
				return _canRead;
			}
		}
		public bool CanWrite {
			get {
				return _canWrite;
			}
		}
		public bool CanExecute {
			get {
				return _canExecute;
			}
		}

		private StandardMemoryPermissions(bool canRead, bool canWrite, bool canExecute) {
			_canRead = canRead;
			_canWrite = canWrite;
			_canExecute = canExecute;

			_cachedPermissionsString = $"{(canRead ? "R" : "-")}{(canWrite ? "W" : "-")}{(canExecute ? "X" : "-")}";
		}

		public override string ToString() {
			return _cachedPermissionsString;
		}

		static private StandardMemoryPermissions?[] _cachedStandardPermissions = new StandardMemoryPermissions[8];

		public static StandardMemoryPermissions GetCached(bool canRead, bool canWrite, bool canExecute) {
			int index = (canRead ? 1 : 0) + (canWrite ? 2 : 0) + (canExecute ? 4 : 0);

			StandardMemoryPermissions? search = _cachedStandardPermissions[index];
			if (search != null) {
				return search;
			} else {
				var newInst = new StandardMemoryPermissions(canRead, canWrite, canExecute);
				_cachedStandardPermissions[index] = newInst;
				return newInst;
			}
		}

	}
}
