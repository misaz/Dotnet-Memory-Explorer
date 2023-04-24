using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {

	public class ProcessEnumerator {

		public IEnumerable<ProcessMetadata> EnumerateProcesses() {

			if (OperatingSystem.IsWindows()) {

				// Inspired at https://social.msdn.microsoft.com/Forums/en-US/669eeaeb-e6fa-403b-86fd-302b24c569fb/how-to-get-the-command-line-arguments-of-running-processes?forum=netfxbcl

				ManagementClass win32ProcessWMI = new ManagementClass("Win32_Process");
				var win32ProcessWMIInstances = win32ProcessWMI.GetInstances();

				List<ProcessMetadata> output = new List<ProcessMetadata>(win32ProcessWMIInstances.Count);

				foreach (ManagementObject process in win32ProcessWMIInstances) {
					UInt32 pidRaw = (uint)process["ProcessId"];

					if (pidRaw > int.MaxValue) {
						throw new Exception("Process id is too high.");
					}

					int pid = (int)pidRaw;
					string name = (string)process["Name"];
					string executable = (string)process["ExecutablePath"];
					string args = (string)process["CommandLine"];
					output.Add(new ProcessMetadata(pid, name, executable, args));
				}

				return output;
			} else {
				throw new PlatformNotSupportedException($"{nameof(ProcessEnumerator)} currently support only Windows platform.");
			}

		}

	}
}
