using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {

	public struct DotnetObjectMetadata {

		public ulong TypeId;

		public ulong Address;

		public DotnetReferenceMetadata[] References;

		public List<DotnetReferenceMetadata> ReferencedBy;

		public DotnetObjectMetadata(ulong typeId, ulong address, DotnetReferenceMetadata[] references) {
			TypeId = typeId;
			Address = address;
			References = references;
			ReferencedBy = new List<DotnetReferenceMetadata>();
		}
	}

}
