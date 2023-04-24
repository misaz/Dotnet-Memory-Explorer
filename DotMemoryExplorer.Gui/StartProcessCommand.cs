using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DotMemoryExplorer.Gui
{
	public class StartProcessCommand : ICommand {
		private ApplicationManager _applicationManager;

		public StartProcessCommand(ApplicationManager applicationManager) {
			this._applicationManager = applicationManager;
		}

		public event EventHandler? CanExecuteChanged;

		public bool CanExecute(object? parameter) {
			return true;
		}

		public void Execute(object? parameter) {
			throw new NotImplementedException();
		}

	}
}
