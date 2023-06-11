namespace InfoManager.Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public IList<SpaceMember> MemberShips { get; set; }
        public IList<Session> Sessions { get; set; }
    }
}
