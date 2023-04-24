using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DotMemoryExplorer.Gui {
	public class ApplicationManager : INotifyPropertyChanged {

		private readonly ObservableCollection<Tab> _tabs = new ObservableCollection<Tab>();
		private Tab? _selectedTab;
		private readonly StartProcessCommand _startProcessCommand;
		private readonly AttachToProcessCommand _attachToProcessCommand;
		private readonly CloseActiveTabCommand _closeTabCommand;


		public StartProcessCommand StartProcessCommand {
			get {
				return _startProcessCommand;
			}
		}

		public AttachToProcessCommand AttachToProcessCommand {
			get {
				return _attachToProcessCommand;
			}
		}

		public CloseActiveTabCommand CloseTabCommand {
			get {
				return _closeTabCommand;
			}
		}

		public ObservableCollection<Tab> Tabs {
			get {
				return _tabs;
			}
		}

		public Tab? SelectedTab {
			get {
				return _selectedTab;
			}
			set {
				if (_selectedTab != value) {

					if (value != null) {
						ThrowIfNotOwnedTab(value);
					}

					// TODO: Store tab selection history for activating recent tabs when closing current tab.

					_selectedTab = value;
					RaisePropertyChanged(nameof(SelectedTab));
				}
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public ApplicationManager() {
			_startProcessCommand = new StartProcessCommand(this);
			_attachToProcessCommand = new AttachToProcessCommand(this);
			_closeTabCommand = new CloseActiveTabCommand(this);
			_selectedTab = null;

			Tabs.Add(new OverviewTab(this));
		}

		private void RaisePropertyChanged(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private void ThrowIfNotOwnedTab(Tab tab) {
			if (!_tabs.Contains(tab)) {
				throw new ArgumentException("Cannot do operation with passed tab because it is not owned by target ApplicationManager.");
			}
		}

		public void CloseTab(Tab selectedTab) {
			if (selectedTab == null) {
				throw new ArgumentNullException(nameof(selectedTab));
			}

			ThrowIfNotOwnedTab(selectedTab);

			_tabs.Remove(selectedTab);
			RaisePropertyChanged(nameof(Tabs));

			SelectedTab = _tabs.FirstOrDefault();
		}

		internal void AddTab(Tab tab, bool autoSelect = true) {
			if (tab == null) {
				throw new ArgumentNullException(nameof(tab));
			}

			if (_tabs.Contains(tab)) {
				throw new InvalidOperationException("Specified tab is already added.");
			}

			_tabs.Add(tab);
			
			RaisePropertyChanged(nameof(Tabs));

			if (autoSelect) {
				SelectedTab = tab;
			}
		}
	}
}
