using DotMemoryExplorer.Core;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace DotMemoryExplorer.Gui {

	public class AttachToProcessCommand : ICommand {

		private readonly ApplicationManager _applicationManager;

		public AttachToProcessCommand(ApplicationManager applicationManager) {
			_applicationManager = applicationManager;
		}

		public event EventHandler? CanExecuteChanged;

		public bool CanExecute(object? parameter) {
			return true;
		}

		public void Execute(object? parameter) {
			ProcessSelectDialog dlg = new ProcessSelectDialog();
			dlg.Title = "Select Process to Attach";

			if (dlg.ShowDialog() == true && dlg.SelectedProcess != null) {
				LiveDotnetProcess process;
				try {
					process = new LiveDotnetProcess(dlg.SelectedProcess.Pid);
				} catch (Exception ex) {
					MessageBox.Show($"Error while attaching to process. Details: {ex.GetType().Name}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				var tab = new ProcessTab(process);

				_applicationManager.AddTab(tab);
			}
		}
	}

}