using DotMemoryExplorer.Core;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	internal class ProcessTab : Tab {

		private readonly IDotnetProcess _process;

		public ProcessTab(IDotnetProcess process, ApplicationManager appManager) : base($"Process {process.Pid} ({process.Name})", new ProcessPane(process, appManager)) { 
			this._process = process;
		}
	}
}