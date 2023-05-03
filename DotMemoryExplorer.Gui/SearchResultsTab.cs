using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	public class SearchResultsTab : Tab {
		public SearchResultsTab(string name, IEnumerable<SearchResult> results, ApplicationManager appManager) : base(name, new SearchResultsPane(results, appManager), true) {
		}
	}
}
