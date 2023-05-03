namespace DotMemoryExplorer.Core.FieldValue
{
    internal class FieldValueNativeUint : FieldContent
    {
        private ulong v;

        public FieldValueNativeUint(ulong v)
        {
            this.v = v;
        }
    }
}