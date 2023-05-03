namespace DotMemoryExplorer.Core.FieldValue
{
    internal class FieldValueInt64 : FieldContent
    {
        private long v;

        public FieldValueInt64(long v)
        {
            this.v = v;
        }
    }
}