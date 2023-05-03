using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Gui {
	public class SearchStringRequest {

		public string SearchTerm { get; }

		public Encoding Encoding { get; }

		public SearchStringRequest(string searchTerm, Encoding encoding) {
			if (searchTerm == null) {
				throw new ArgumentNullException(nameof(searchTerm));
			}

			if (encoding == null) {
				throw new ArgumentNullException(nameof(encoding));
			}

			SearchTerm = searchTerm;
			Encoding = encoding;
		}
	}
}
