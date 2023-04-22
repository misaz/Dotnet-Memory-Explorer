using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {

	/// <summary>
	/// Abstraction of DotnetProcess with support for providing PropertyChanged events for at least following properties:
	/// - CanDump
	/// - AvalaibleDumps
	/// </summary>
	public interface IDotnetProcess : INotifyPropertyChanged {
		public int Pid { get; }

		/// <summary>
		/// Indication that process can be dumped by MakeDump()
		/// </summary>
		public bool CanDump { get; }

		public IEnumerable<HeapDump> AvalaibleDumps { get; }

		public HeapDump MakeDump();
	}

}
