namespace TinyTODO.Core.DataModel
{
    public class Setting
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Setting()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }

        public string Key { get; set; }

        public bool? ValueBoolean { get; internal set; }
        public string? ValueString { get; internal set; }
        public double? ValueDouble { get; internal set; }
        public int? ValueInteger { get; internal set; }
        public DateTime? ValueDateTime { get; internal set; }
    }
}
