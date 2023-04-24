using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	public class OverviewTab : Tab {

		private readonly ApplicationManager _manager;

		public OverviewTab(ApplicationManager applicationManager) : base("Overview", new OverviewPane(applicationManager), false) {
			this._manager = applicationManager;
		}

	}
}
