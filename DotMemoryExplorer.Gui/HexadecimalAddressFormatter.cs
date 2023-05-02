using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DotMemoryExplorer.Gui {
	public class HexadecimalAddressConverter : IValueConverter {

		public static HexadecimalAddressConverter Shared = new HexadecimalAddressConverter();

		public string Convert(ulong address) {
			if (Environment.Is64BitProcess) {
				return $"0x{address.ToString("X16")}";
			} else {
				return $"0x{address.ToString("X8")}";
			}

		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is ulong) {
				return Convert((ulong)value);
			} else {
				throw new ArgumentException("Unable to convert passsed value because it is value of unexpected type.");
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
