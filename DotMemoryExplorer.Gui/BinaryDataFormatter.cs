using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.Gui {
	public class BinaryDataFormatter {

		public static string FormatBinary(ReadOnlySpan<byte> data) {
			int offsetLength = data.Length.ToString("X").Length;
			if (offsetLength < 4) {
				offsetLength = 4;
			}

			StringBuilder sb = new StringBuilder();
			StringBuilder sbString = new StringBuilder();
			sbString.Append("   ");
			int i = 0;
			for (; i < data.Length; i++) {
				if (i % 16 == 0) {
					sb.Append(i.ToString("X").PadLeft(offsetLength, '0'));
					sb.Append("    ");
				}

				sb.Append(data[i].ToString("X2"));

				if (Environment.Is64BitProcess) {
					if (i % 8 == 7) {
						sb.Append(" ");
					}
				} else {
					if (i % 4 == 3) {
						sb.Append(" ");
					}
				}

				char chr = BitConverter.ToChar(new byte[2] { data[i], 0 }, 0);
				if (char.IsWhiteSpace(chr) || char.IsControl(chr)) {
					sbString.Append(".");
				} else {
					sbString.Append(chr);
				}

				if (i > 0 && i % 16 == 15) {
					sb.Append(sbString.ToString());
					sb.Append('\n');

					sbString.Clear();
					sbString.Append("   ");
				} else {
					sb.Append(' ');
				}
			}

			// complete last line by spaces and add (even partial) string formated data
			while (i % 16 != 0) {
				sb.Append("  ");

				if (Environment.Is64BitProcess) {
					if (i % 8 == 7) {
						sb.Append(" ");
					}
				} else {
					if (i % 4 == 3) {
						sb.Append(" ");
					}
				}

				sbString.Append(" ");

				if (i > 0 && i % 16 == 15) {
					sb.Append(sbString.ToString());
					sb.Append('\n');
				} else {
					sb.Append(' ');
				}

				i++;
			}

			return sb.ToString();

		}

	}
}
