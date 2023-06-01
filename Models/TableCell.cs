namespace InfoManager.Models
{
    public class TableCell
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public int RowId { get; set; }
        public TableRow Row { get; set; }
        public int FieldId { get; set; }
        public TableField Field { get; set; }
    }
}
