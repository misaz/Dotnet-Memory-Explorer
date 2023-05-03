namespace DotMemoryExplorer.Core.FieldValue
{
    internal class FieldValueNativeInt : FieldContent
    {
        private long v;

        public FieldValueNativeInt(long v)
        {
            this.v = v;
        }
    }
}