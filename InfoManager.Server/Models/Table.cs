namespace InfoManager.Server.Models
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SpaceId { get; set; }
        public Space Space { get; set; }

        public IList<TableField> Fields { get; set; }
        public IList<TableRow> Rows { get; set; }
    }
}
