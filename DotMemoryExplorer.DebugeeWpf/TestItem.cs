using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotMemoryExplorer.DebugeeWpf
{
    public class TestItem
    {
        public string? Name;
        public int Value;
        public TestItem? Reference;

        public int GetMagicNumber() {
            if (Name == null) {
                return Value + 42;
            } else {
                return Name.Length + Value;
            }
        }
    }
}
