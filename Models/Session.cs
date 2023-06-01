namespace InfoManager.Models
{
    public class Session
    {
        public int Id { get; set; }
        public byte[] Key { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
