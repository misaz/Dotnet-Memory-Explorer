using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.System.Memory;

namespace DotMemoryExplorer.Core {
	public class WindowsMemoryType : IMemoryType {

		public static WindowsMemoryType Image = new WindowsMemoryType(PAGE_TYPE.MEM_IMAGE, "Image");
		public static WindowsMemoryType Mapped = new WindowsMemoryType(PAGE_TYPE.MEM_MAPPED, "Mapped");
		public static WindowsMemoryType Private = new WindowsMemoryType(PAGE_TYPE.MEM_PRIVATE, "Private");

		private readonly PAGE_TYPE _type;
		private readonly string _name;

		private WindowsMemoryType(PAGE_TYPE type, string name) {
			_type = type;
			_name = name;
		}

		private static Dictionary<PAGE_TYPE, WindowsMemoryType> _cachedTypes = new Dictionary<PAGE_TYPE, WindowsMemoryType> {
			{PAGE_TYPE.MEM_IMAGE, Image },
			{PAGE_TYPE.MEM_MAPPED, Mapped },
			{PAGE_TYPE.MEM_PRIVATE, Private },
		};

		public static WindowsMemoryType GetCached(uint type) {
			PAGE_TYPE pageType = (PAGE_TYPE)type;
			if (_cachedTypes.ContainsKey(pageType)) {
				return _cachedTypes[pageType];
			} else {
				var inst = new WindowsMemoryType(pageType, $"Type {type}");
				_cachedTypes.Add(pageType, inst);
				return inst;
			}

		}
	}
}
