namespace InfoManager.Server.Models
{
    public class TableRow
    {
        public int Id { get; set; }

        public int TableId { get; set; }
        public Table Table { get; set; }
        public IList<TableCell> Cells { get; set; }
    }
}
