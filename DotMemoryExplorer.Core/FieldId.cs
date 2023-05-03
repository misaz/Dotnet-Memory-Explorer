using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Core {
	public struct FieldId : IComparable {

		public ulong OwnerTypeId { get; }
		public ulong Mb { get; }

		public FieldId(ulong ownerTypeId, ulong mb) {
			OwnerTypeId = ownerTypeId;
			Mb = mb;
		}

		public bool AreSame(FieldId fieldId) {
			return this.OwnerTypeId == fieldId.OwnerTypeId && this.Mb == fieldId.Mb;
		}

		public int CompareTo(object? obj) {
			if (obj is not FieldId) {
				throw new InvalidOperationException($"Only two {obj} can be compared.");
			}

			FieldId fid2 = (FieldId)obj;

			int comp1 = OwnerTypeId.CompareTo(fid2.OwnerTypeId);
			if (comp1 == 0) {
				return Mb.CompareTo(fid2.Mb);
			} else {
				return comp1;
			}
		}
	}
}
