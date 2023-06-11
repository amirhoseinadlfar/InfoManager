namespace InfoManager.Server.Models
{
    public class TableField
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FieldType FieldType { get; set; }

        public int TableId { get; set; }
        public Table Table { get; set; }

        public IList<TableCell> Cells { get; set; }
    }
    public enum FieldType : byte
    {
        Byte = 0,
        SByte = 1,
        UInt16 = 2,
        Int16 = 3,
        UInt32 = 4,
        Int32 = 5,
        UInt64 = 6,
        Int64 = 7,
        Float = 8,
        Double = 9,
        Decimal = 10,

        Char = 11,
        String = 12,
    }
}
