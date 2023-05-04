using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {
	
	public class NtSuspendBlock : IDisposable {
		private readonly IntPtr _processHandle;

		[DllImport("ntdll.dll", PreserveSig = false)]
		public static extern void NtSuspendProcess(IntPtr processHandle);

		[DllImport("ntdll.dll", PreserveSig = false, SetLastError = true)]
		public static extern void NtResumeProcess(IntPtr processHandle);

		public NtSuspendBlock(Process p) {
			_processHandle = p.Handle;
			NtSuspendProcess(_processHandle);
		}

		public void Dispose() {
			NtResumeProcess(_processHandle);
		}
	}
}
