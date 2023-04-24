using System;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {

	public partial class OverviewPane : UserControl {

		private readonly ApplicationManager _applicationManager;

		public ApplicationManager ApplicationManager {
			get {
				return _applicationManager;
			}
		}

		public OverviewPane(ApplicationManager applicationManager) {
			if (applicationManager == null) {
				throw new ArgumentNullException(nameof(applicationManager));
			}

			_applicationManager = applicationManager;

			this.DataContext = this;
			InitializeComponent();

			var version = this.GetType().Assembly.GetName().Version;
			if (version != null) {
				txtVersion.Text = $"Version: {version.ToString()}";
			} else {
				txtVersion.Text = "";
			}
		}
	}

}
