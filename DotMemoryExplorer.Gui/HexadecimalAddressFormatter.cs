using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DotMemoryExplorer.Gui {
	public class HexadecimalAddressConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is ulong) {
				if (Environment.Is64BitProcess) {
					return $"0x{((ulong)value).ToString("X16").PadLeft(16, '0')}";
				} else {
					return $"0x{((ulong)value).ToString("X8").PadLeft(8, '0')}";
				}
			} else {
				throw new ArgumentException("Unable to convert passsed value because it is value of unexpected type.");
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
