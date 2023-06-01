namespace InfoManager.Models
{
    public class Space
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<SpaceMember> Members { get; set; }
        public IList<Table> Tables { get; set; }
    }
}
