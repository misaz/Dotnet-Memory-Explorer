using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	public class Tab : INotifyPropertyChanged {

		public string Name { get; }
		public bool CanClose { get; }
		public Control Content { get; }

		public event PropertyChangedEventHandler? PropertyChanged;

		public Tab(string name, Control content, bool canClose = true) {
			if (name == null) {
				throw new ArgumentNullException(nameof(name));
			}
			if (content == null) {
				throw new ArgumentNullException(nameof(content));
			}

			Name = name;
			CanClose = canClose;
			Content = content;
		}
	}
}
