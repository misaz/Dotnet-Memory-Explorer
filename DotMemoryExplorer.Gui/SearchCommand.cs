using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DotMemoryExplorer.Gui {

	public abstract class SearchCommand : ICommand {

		protected readonly ApplicationManager _applicationManager;

		public SearchCommand(ApplicationManager applicationManager) {
			if (applicationManager == null) {
				throw new ArgumentNullException(nameof(applicationManager));
			}

			_applicationManager = applicationManager;

			_applicationManager.PropertyChanged += ApplicationManager_PropertyChanged;
		}

		private void ApplicationManager_PropertyChanged(object? sender, PropertyChangedEventArgs e) {
			if (e.PropertyName == nameof(ApplicationManager.SelectedTab)) {
				RaiseCanExecuteChanged();
			}
		}

		private void RaiseCanExecuteChanged() {
			if (CanExecuteChanged != null) {
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}

		protected HeapDumpTab GetHeapDumpTab() {
			if (_applicationManager.SelectedTab is not HeapDumpTab) {
				throw new InvalidOperationException("Unable to search dump when non-dump tab is selected.");
			}

			return (HeapDumpTab)_applicationManager.SelectedTab;
		}

		public event EventHandler? CanExecuteChanged;

		public bool CanExecute(object? parameter) {
			return _applicationManager.SelectedTab is HeapDumpTab;
		}

		public abstract void Execute(object? parameter);

	}
}
