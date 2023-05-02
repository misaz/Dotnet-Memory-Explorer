using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace DotMemoryExplorer.Core {
	public class LiveDotnetProcess : IDotnetProcess, IDisposable {
		private int _pid;
		private bool _canDump;
		private Process _process;
		private Lazy<int> _bitnessLazy;
		private List<HeapDump> _dumps = new List<HeapDump>();
		private IHeapDumperFactory _dumperFactory;
		private IProcessMemoryManger _memoryManger;

		public event PropertyChangedEventHandler? PropertyChanged;

		public IEnumerable<HeapDump> AvalaibleDumps {
			get {
				return _dumps.AsReadOnly();
			}
		}

		public int Pid {
			get {
				return _pid;
			}
		}

		public string Name {
			get {
				return _process.ProcessName;
			}
		}

		public bool CanDump {
			get {
				return _canDump;
			}
		}

		public IHeapDumperFactory DumperFactory {
			get {
				return _dumperFactory;
			}
			set {
				if (value == null) {
					throw new ArgumentNullException(nameof(value));
				}

				_dumperFactory = value;
			}
		}

		public IProcessMemoryManger ProcessMemoryManger {
			get {
				return _memoryManger;
			}
		}

		public unsafe int Bitness {
			get {
				return _bitnessLazy.Value;
			}
		}

		public LiveDotnetProcess(int pid) {
			try {
				_process = Process.GetProcessById(pid);
				_process.Exited += process_Exited;

				if (!_process.HasExited) {
					_canDump = true;
				}
			} catch (Exception ex) {
				throw new ArgumentException($"Unable to acquire Process with PID={pid}. Details: {ex.GetType().Name}: {ex.Message}");
			}

			_bitnessLazy = new Lazy<int>(DetermineTargetProcessBitness);
			_pid = pid;
			_memoryManger = new WindowsProcessMemoryManger(_process);

			// EtwHeapDumper is default strategy for dumping process. User can change it by writing DumperFactory property.
			_dumperFactory = new EtwHeapDumperFactory(this);
		}

		private int DetermineTargetProcessBitness() {
			if (!Environment.Is64BitOperatingSystem) {
				return 32;
			}

			BOOL isWow64;
			int status = PInvoke.IsWow64Process(_process.SafeHandle, out isWow64);
			if (status == 0) {
				throw new Exception($"Unable te get target process bittness. IsWow64Process failed with status code {Marshal.GetLastWin32Error()}");
			}

			if (isWow64) {
				return 32;
			} else {
				return 64;
			}
		}

		private void process_Exited(object? sender, EventArgs e) {
			_canDump = false;
			RaisePropertyChanged(nameof(CanDump));
		}

		HeapDump IDotnetProcess.MakeDump() {
			using (IHeapDumper dumper = DumperFactory.CreateDumper()) {
				HeapDump dump = dumper.MakeDump();

				_dumps.Add(dump);

				RaisePropertyChanged(nameof(AvalaibleDumps));

				return dump;
			}
		}

		private void RaisePropertyChanged(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public override string ToString() {
			return $"{Name} (PID: {Pid})";
		}

		public void Dispose() {
			_process.Exited -= process_Exited;
		}
	}
}
