namespace DotMemoryExplorer.Core.FieldValue
{
    internal class FieldValueUInt64 : FieldContent
    {
        private ulong v;

        public FieldValueUInt64(ulong v)
        {
            this.v = v;
        }
    }
}