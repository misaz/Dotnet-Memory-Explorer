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

		private string _name;

		public string Name {
			get {
				return _name;
			}
			protected set {
				if (value ==  null) {
					throw new ArgumentNullException(nameof(value));
				}

				_name = value;
				RaiseNotifyPropertyChanged(nameof(Name));
			}
		}

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

			_name = name;
			CanClose = canClose;
			Content = content;
		}

		private void RaiseNotifyPropertyChanged(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
