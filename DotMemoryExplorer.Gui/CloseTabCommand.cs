using System;
using System.ComponentModel;
using System.Windows.Input;

namespace DotMemoryExplorer.Gui {
	public class CloseActiveTabCommand : ICommand {

		private readonly ApplicationManager _applicationManager;

		public CloseActiveTabCommand(ApplicationManager applicationManager) {
			this._applicationManager = applicationManager;

			this._applicationManager.PropertyChanged += applicationManager_PropertyChanged;
		}

		private void applicationManager_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(ApplicationManager.SelectedTab)) {
				RaiseCanExecuteChanged(nameof(CanExecuteChanged));
			}
		}


		public event EventHandler? CanExecuteChanged;

		public bool CanExecute(object? parameter) {
			if (_applicationManager.SelectedTab == null) {
				return false;
			}

			if (!_applicationManager.SelectedTab.CanClose) {
				return false;
			}

			return true;
		}

		public void Execute(object? parameter) {
			_applicationManager.CloseTab(_applicationManager.SelectedTab);
		}

		private void RaiseCanExecuteChanged(string v) {
			if (CanExecuteChanged != null) {
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}
	}
}