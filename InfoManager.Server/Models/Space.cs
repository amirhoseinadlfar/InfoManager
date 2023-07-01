namespace InfoManager.Server.Models
{
    public class Space
    {
        public const int NameMinLength = 1;
        public const int NameMaxLength = 15;

        public int Id { get; set; }
        public string Name { get; set; }
        public IList<SpaceMember> Members { get; set; }
        public IList<Table> Tables { get; set; }
    }
}
