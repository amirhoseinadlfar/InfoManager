namespace InfoManager.Server.Models
{
    public class User
    {
        public const int UsernameMinLength = 4;
        public const int UsernameMaxLength = 16;

        public const int NameMinLength = 4;
        public const int NameMaxLength = 16;

        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 32;

        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public IList<Space> Spaces { get; set; }
        public IList<SpaceMember> MemberShips { get; set; }
        public IList<Session> Sessions { get; set; }
    }
}
