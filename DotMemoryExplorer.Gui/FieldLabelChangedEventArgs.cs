using DotMemoryExplorer.Core;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;

namespace DotMemoryExplorer.Gui {
	public class FieldLabelChangedEventArgs {

		public FieldId FieldId;

		public FieldLabelChangedEventArgs(FieldId fieldId) {
			FieldId = fieldId;
		}
	}
}