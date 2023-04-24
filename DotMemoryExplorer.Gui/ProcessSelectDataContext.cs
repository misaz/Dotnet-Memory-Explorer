using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DotMemoryExplorer.Gui {

	class ProcessSelectDataContext : INotifyPropertyChanged {

		public event PropertyChangedEventHandler? PropertyChanged;

		private readonly ProcessEnumerator _processEnumerator = new ProcessEnumerator();
		private string _searchTerm = "";
		private readonly List<ProcessMetadata> _allProceses = new List<ProcessMetadata>();
		private List<ProcessMetadata> _visibleProcesses;
		private ProcessMetadata? _selectedProcess;

		public IEnumerable<ProcessMetadata> VisibleProcesses {
			get {
				return _visibleProcesses.AsReadOnly();
			}
		}

		public string SearchText {
			get {
				return _searchTerm;
			}
			set {
				if (value == null) {
					throw new ArgumentNullException(nameof(value));
				}

				_searchTerm = value;
				Filter();
			}
		}

		public ProcessMetadata? SelectedProcess {
			get {
				return _selectedProcess;
			}
			set {
				_selectedProcess = value;
			}
		}

		public ProcessSelectDataContext() {
			_visibleProcesses = _allProceses;
			_selectedProcess = null;
			RefreshProcesses();
		}

		public void RefreshProcesses() {
			IEnumerable<ProcessMetadata> enumeratedProcesses;
				enumeratedProcesses = _processEnumerator.EnumerateProcesses();
			try {
			} catch {
				enumeratedProcesses = Enumerable.Empty<ProcessMetadata>();
			}

			_allProceses.Clear();
			_allProceses.AddRange(enumeratedProcesses);
			Filter();
		}

		private void Filter() {
			if (string.IsNullOrWhiteSpace(_searchTerm)) {
				_visibleProcesses = _allProceses;
				RaisePropertyChanged(nameof(VisibleProcesses));
				return;
			}

			var searchTermLower = _searchTerm.ToLowerInvariant();

			List<ProcessMetadata> filtered = new List<ProcessMetadata>();
			foreach (ProcessMetadata process in _allProceses) {
				if (searchTermLower.Contains(process.Pid.ToString())) {
					filtered.Add(process);
					continue;
				}

				if (process.Name.ToLowerInvariant().Contains(searchTermLower)) {
					filtered.Add(process);
					continue;
				}

				if (process.ExecutablePath != null && process.ExecutablePath.ToLowerInvariant().Contains(searchTermLower)) {
					filtered.Add(process);
					continue;
				}

				if (process.CommandLine != null && process.CommandLine.ToLowerInvariant().Contains(searchTermLower)) {
					filtered.Add(process);
					continue;
				}
			}

			_visibleProcesses = filtered;
			RaisePropertyChanged(nameof(VisibleProcesses));
			return;
		}

		private void RaisePropertyChanged(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}

}
