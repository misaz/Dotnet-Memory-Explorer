namespace DotMemoryExplorer.Core.FieldValue
{
    internal class FieldValuePointer : FieldContent
    {
        private ulong v;

        public FieldValuePointer(ulong v)
        {
            this.v = v;
        }
    }
}