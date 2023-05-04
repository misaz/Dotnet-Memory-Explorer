using DotMemoryExplorer.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace DotMemoryExplorer.Gui {
	public class SearchStringCommand : SearchCommand {

		public SearchStringCommand(ApplicationManager applicationManager) : base(applicationManager) {
		}

		public override void Execute(object? parameter) {
			HeapDumpTab heapDumpTab = GetHeapDumpTab(); 

			var dlg = new SearchStringWindow();
			if (dlg.ShowDialog() == true && dlg.Request != null) {
				IEnumerable<SearchResult> res = heapDumpTab.HeapDump.SearchEngine.SearchString(dlg.Request);
				SearchResultsTab searchResultsTab = new SearchResultsTab($"'{dlg.Request.SearchTerm}' (len {dlg.Request.SearchTerm.Length}) Search Results", res, _applicationManager);
				_applicationManager.AddTab(searchResultsTab);
			}
		}

	}
}