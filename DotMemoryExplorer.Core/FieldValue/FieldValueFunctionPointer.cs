namespace DotMemoryExplorer.Core.FieldValue
{
    internal class FieldValueFunctionPointer : FieldContent
    {
        private ulong v;

        public FieldValueFunctionPointer(ulong v)
        {
            this.v = v;
        }
    }
}