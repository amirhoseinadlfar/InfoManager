namespace InfoManager.Server.Models
{
    public class TableField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FieldType FieldType { get; set; }
        public int Limit { get; set; }

        public int TableId { get; set; }
        public Table Table { get; set; }

        public IList<TableCell> Cells { get; set; }
    }
    public enum FieldType : byte
    {
        ShortString,
        LongString,
        Enum,

        Bool,
        Byte,
        Short,
        Int,
        Long,
        Float,
        Double,
        Decimal,

        Date,
        Time,
        DateTime,

    }
}
