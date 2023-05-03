using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	public class GuiEventsHelper {

		public static T UnpackSenderTag<T>(object sender) {
			if (sender is not Control) {
				throw new InvalidOperationException("Unable to process event because event is fired by non-Control.");
			}

			Control c = (Control)sender;

			if (c.Tag is not T) {
				throw new InvalidOperationException("Unable to process event because it is fired by control with invalid Tag set.");
			}

			return (T)c.Tag;
		}
	}
}
