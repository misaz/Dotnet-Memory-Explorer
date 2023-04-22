using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {

	public interface IHeapDumperFactory {
		public IHeapDumper CreateDumper();
	}

}
