namespace InfoManager.Server.Models
{
    public class Table
    {
        public const int NameMinLength = 1;
        public const int NameMaxLength = 15;

        public int Id { get; set; }
        public string Name { get; set; }
        public int SpaceId { get; set; }
        public Space Space { get; set; }

        public IList<TableField> Fields { get; set; }
        public IList<TableRow> Rows { get; set; }
    }
}
