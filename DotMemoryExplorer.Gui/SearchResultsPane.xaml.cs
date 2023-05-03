using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	/// <summary>
	/// Interaction logic for SearchResultsPane.xaml
	/// </summary>
	public partial class SearchResultsPane : UserControl {

		public IEnumerable<SearchResult> Results { get; }
		public ApplicationManager AppManager { get; }

		public SearchResultsPane(IEnumerable<SearchResult> results, ApplicationManager appManager) {
			if (results is null) {
				throw new ArgumentNullException(nameof(results));
			}

			if (appManager is null) {
				throw new ArgumentNullException(nameof(appManager));
			}

			Results = results;
			AppManager = appManager;

			DataContext = this;
			InitializeComponent();
		}

		private void DataGridRow_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
			SearchResult sr = GuiEventsHelper.UnpackSenderTag<SearchResult>(sender);

			// skip double clicks on SearchResult entries which do not hold reference to any object
			if (sr is not SearchResultAddressInObject) {
				return;
			}

			SearchResultAddressInObject searchResultWithObject = (SearchResultAddressInObject)sr;

			AppManager.OpenObjectDetail(searchResultWithObject.ObejctMetadata, searchResultWithObject.OwningHeapDump);

		}
    }
}
