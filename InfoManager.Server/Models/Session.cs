namespace InfoManager.Server.Models
{
    public class Session
    {
        public const int KeyLength = 64;

        public int Id { get; set; }
        public string Key { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
