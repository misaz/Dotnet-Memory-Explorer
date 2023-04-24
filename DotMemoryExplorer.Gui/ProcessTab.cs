using DotMemoryExplorer.Core;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	internal class ProcessTab : Tab {

		private readonly IDotnetProcess _process;

		public ProcessTab(IDotnetProcess process) : base($"Process {process.Pid} ({process.Name})", new Button()) { 
			this._process = process;
		}
	}
}