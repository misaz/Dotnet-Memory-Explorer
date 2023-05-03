using DotMemoryExplorer.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DotMemoryExplorer.Gui {
	public class SearchResultToObjectInfoConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is not SearchResult) {
				throw new ArgumentException($"Only instenaces of {nameof(SearchResult)} are supported by {nameof(SearchResultToObjectInfoConverter)}");
			}

			if (value is SearchResultAddress) {
				return $"Occurence is in non-heap segment or containing object is collected";
			}

			if (value is SearchResultAddressInObject) {
				SearchResultAddressInObject sr = (SearchResultAddressInObject)value;
				return $"{HexadecimalAddressConverter.Shared.Convert(sr.OccurenceAddress)} ({sr.OwningHeapDump.GetTypeById(sr.ObejctMetadata.TypeId).TypeName})";
			}

			return "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
