﻿using System;
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
		public string Name { get; }

		public IProcessMemoryManger ProcessMemoryManger { get; }

		/// <summary>
		/// Indication that process can be dumped by MakeDump()
		/// </summary>
		public bool CanDump { get; }

		public IEnumerable<HeapDump> AvalaibleDumps { get; }
		int Bitness { get; }

		public HeapDump MakeDump();
	}

}
