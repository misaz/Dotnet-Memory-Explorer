using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DotMemoryExplorer.Gui {
	/// <summary>
	/// Interaction logic for SearchBinaryWindow.xaml
	/// </summary>
	public partial class SearchBinaryWindow : Window, INotifyPropertyChanged {
		private bool _isValid = false;
		private byte[] _parsed = new byte[0];
		private string _searchTerm = string.Empty;

		public event PropertyChangedEventHandler? PropertyChanged;

		public string SearchTerm {
			get {
				return _searchTerm;
			}
			set {
				if (value == null) {
					throw new ArgumentNullException(nameof(value));
				}

				_searchTerm = value;
				UpdatePreview();
			}
		}

		public string SearchFormatted {
			get {
				if (!_isValid) {
					return "Input is not valid";
				} else {
					return BinaryDataFormatter.FormatBinary(_parsed.AsSpan());
				}
			}
		}

		public bool SearchEnabled {
			get {
				return _isValid;
			}
		}

		public ReadOnlySpan<byte> SearchTermBinary {
			get {
				return new ReadOnlySpan<byte>(_parsed);
			}
		}

		private void UpdatePreview() {
			string[] parts = SearchTerm.ToLowerInvariant().Replace("0x", "").Replace(".", " ").Replace(",", " ").Replace(";", " ").Split();

			List<byte> mem = new List<byte>(parts.Length);

			foreach (var part in parts) {
				if (string.IsNullOrWhiteSpace(part)) {
					continue;
				}

				if (part.Length < 2) {
					try {
						mem.Add(Convert.ToByte(part, 16));
					} catch {
						_isValid = false;
						RaisePropertyChanged(nameof(SearchFormatted));
						RaisePropertyChanged(nameof(SearchEnabled));
						return;
					}
				} else {
					string workingPart = part;
					while (workingPart.Length >= 2) {
						string b = workingPart.Substring(workingPart.Length - 2, 2);
						try {
							mem.Add(Convert.ToByte(b, 16));
						} catch {
							_isValid = false;
							RaisePropertyChanged(nameof(SearchFormatted));
							RaisePropertyChanged(nameof(SearchEnabled));
							return;
						}
						workingPart = workingPart.Substring(0, workingPart.Length - 2);
					}
					if (workingPart.Length == 1) {
						try {
							mem.Add(Convert.ToByte(workingPart, 16));
						} catch {
							_isValid = false;
							RaisePropertyChanged(nameof(SearchFormatted));
							RaisePropertyChanged(nameof(SearchEnabled));
							return;
						}
					}
				}
			}

			_parsed = mem.ToArray();
			_isValid = mem.Count > 0;

			RaisePropertyChanged(nameof(SearchFormatted));
			RaisePropertyChanged(nameof(SearchEnabled));
		}

		public SearchBinaryWindow() {
			DataContext = this;
			InitializeComponent();
		}

		private void RaisePropertyChanged(string propertyName) {
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private void Search_Click(object sender, RoutedEventArgs e) {
			if (!_isValid) {
				throw new InvalidOperationException("Cannot search when input is invalid");
			}

			DialogResult = true;
		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			DialogResult = false;
		}
	}
}
