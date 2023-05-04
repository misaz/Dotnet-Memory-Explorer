using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DotMemoryExplorer.Gui {
	public class SearchBinaryCommand : SearchCommand {

		public SearchBinaryCommand(ApplicationManager applicationManager) : base(applicationManager) {
		}

		public override void Execute(object? parameter) {
			HeapDumpTab heapDumpTab = GetHeapDumpTab();

			var dlg = new SearchBinaryWindow();
			if (dlg.ShowDialog() == true) {
				IEnumerable<SearchResult> res = heapDumpTab.HeapDump.SearchEngine.Search(dlg.SearchTermBinary);
				string tabHeader;

				if (dlg.SearchTermBinary.Length > 6) {
					string starting = FormatTabHeaderBytes(dlg.SearchTermBinary.Slice(0, 3));
					string tailing = FormatTabHeaderBytes(dlg.SearchTermBinary.Slice(dlg.SearchTermBinary.Length - 3));
					tabHeader = $"'{starting} ... {tailing}' Binary Search";
				} else {
					string content = FormatTabHeaderBytes(dlg.SearchTermBinary);
					tabHeader = $"'{content}' Binary Search";
				}

				SearchResultsTab searchResultsTab = new SearchResultsTab(tabHeader, res, _applicationManager);
				_applicationManager.AddTab(searchResultsTab);
			}
		}

		private string FormatTabHeaderBytes(ReadOnlySpan<byte> bytes) {
			return string.Join(" ", from x in bytes.ToArray()
									select x.ToString("X2"));
		}


	}
}
