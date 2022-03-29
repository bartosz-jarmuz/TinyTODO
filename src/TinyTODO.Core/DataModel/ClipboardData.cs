namespace TinyTODO.Core.DataModel
{
    public record ClipboardData
    {
        public ClipboardData(ClipboardDataType dataType, object data)
        {
            DataType = dataType;
            Data = data;
        }

        public ClipboardDataType DataType { get; }
        public object Data { get; }

        public override string ToString()
        {
            return Data?.ToString()??"[NoData]";
        }
    }
}
