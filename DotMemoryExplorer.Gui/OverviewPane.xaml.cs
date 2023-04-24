using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {

	public partial class OverviewPane : UserControl {
		public OverviewPane() {
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
