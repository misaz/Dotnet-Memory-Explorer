using DotMemoryExplorer.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace DotMemoryExplorer.Gui {
	public class SearchStringCommand : ICommand {

		private readonly ApplicationManager _applicationManager;

		public SearchStringCommand(ApplicationManager applicationManager) {
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

		public event EventHandler? CanExecuteChanged;

		public bool CanExecute(object? parameter) {
			return _applicationManager.SelectedTab is HeapDumpTab;
		}

		public void Execute(object? parameter) {
			if (_applicationManager.SelectedTab is not HeapDumpTab) {
				throw new InvalidOperationException("Unable to search dump when non-dump tab is selected.");
			}

			HeapDumpTab heapDumpTab = (HeapDumpTab)_applicationManager.SelectedTab;

			var dlg = new SearchStringWindow();
			if (dlg.ShowDialog() == true && dlg.Request != null) {
				IEnumerable<SearchResult> res = heapDumpTab.HeapDump.SearchEngine.SearchString(dlg.Request);
				SearchResultsTab searchResultsTab = new SearchResultsTab($"'{dlg.Request.SearchTerm}' (len {dlg.Request.SearchTerm.Length}) Search Results", res, _applicationManager);
				_applicationManager.AddTab(searchResultsTab);
			}
		}
	}
}