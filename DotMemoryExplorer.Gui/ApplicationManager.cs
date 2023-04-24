using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Gui {
	public class ApplicationManager : INotifyPropertyChanged {

		public event PropertyChangedEventHandler? PropertyChanged;

		private ObservableCollection<Tab> _tabs= new ObservableCollection<Tab>();

		public ObservableCollection<Tab> Tabs {
			get {
				return _tabs;
			}
		}

		public ApplicationManager() {
			Tabs.Add(new OverviewTab(this));
		}

	}
}
