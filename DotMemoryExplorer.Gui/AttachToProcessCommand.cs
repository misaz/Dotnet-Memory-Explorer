using System;
using System.Windows.Input;

namespace DotMemoryExplorer.Gui {

	public class AttachToProcessCommand : ICommand {

		private readonly ApplicationManager _applicationManager;

		public AttachToProcessCommand(ApplicationManager applicationManager) {
			_applicationManager = applicationManager;
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