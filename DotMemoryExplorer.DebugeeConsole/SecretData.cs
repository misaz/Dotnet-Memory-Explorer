using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.DebugeeConsole {
	internal class SecretData {

		public string Text { get; }

		public int Number { get; }

		public SecretData? Prev { get; }

		public SecretData(string text, int number, SecretData? prev) {
			Text = text;
			Number = number;
			this.Prev = prev;
		}
	}
}
