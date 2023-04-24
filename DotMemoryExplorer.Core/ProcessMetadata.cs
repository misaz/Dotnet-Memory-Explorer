using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {

	/// <summary>
	/// Class used for storing cached meratadata from System.Process class which is internaly supported by system calls which are slow.
	/// </summary>
	public class ProcessMetadata {

		private int _pid;
		private string _name;
		private string? _executablePath;
		private string? _commandLine;

		public int Pid {
			get {
				return _pid;
			}
		}
		public string Name {
			get {
				return _name;
			}
		}
		public string? ExecutablePath {
			get {
				return _executablePath;
			}
		}
		public string? CommandLine {
			get {
				return _commandLine;
			}
		}

		public ProcessMetadata(int pid, string name, string? executablePath, string? commandLine) {
			if (name == null) {
				throw new ArgumentNullException(nameof(name));
			}

			if (name == null) {
				throw new ArgumentNullException(nameof(name));
			}

			_pid = pid;
			_name = name;
			_executablePath = executablePath;
			_commandLine = commandLine;
		}
	}
}
