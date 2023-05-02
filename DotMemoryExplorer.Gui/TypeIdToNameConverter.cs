using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DotMemoryExplorer.Gui {
	public class TypeIdToNameConverter : IMultiValueConverter {

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
			if (values.Length != 2) {
				throw new ArgumentException($"{nameof(TypeIdToNameConverter)} expectes two values for conversion.");
			}

			if (values[0] is not ulong) {
				throw new ArgumentException("Converter can convert only ulong type ids to name. First value must be ulong.");
			}

			if (values[1] is not HeapDump) {
				throw new ArgumentException("Second value must be HeapDump used as base for type anme resolving.");
			}

			ulong val = (ulong)values[0];
			HeapDump heapDump = (HeapDump)values[1];

			return heapDump.GetTypeById(val).TypeName;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
