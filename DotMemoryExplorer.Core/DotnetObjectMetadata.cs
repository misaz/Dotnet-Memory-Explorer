using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {

	public struct DotnetObjectMetadata {

		public ulong TypeId { get; }
		public ulong Address { get; }
		public DotnetReferenceMetadata[] References { get; }
		public List<DotnetReferenceMetadata> ReferencedBy { get; }

		public DotnetObjectMetadata(ulong typeId, ulong address, DotnetReferenceMetadata[] references) {
			TypeId = typeId;
			Address = address;
			References = references;
			ReferencedBy = new List<DotnetReferenceMetadata>();
		}
	}

}
