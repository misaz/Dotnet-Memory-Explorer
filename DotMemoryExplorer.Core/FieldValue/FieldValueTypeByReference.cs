namespace DotMemoryExplorer.Core.FieldValue
{
    internal class FieldValueTypeByReference : FieldContent
    {
        private ulong v;

        public FieldValueTypeByReference(ulong v)
        {
            this.v = v;
        }
    }
}