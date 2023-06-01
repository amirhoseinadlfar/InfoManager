namespace InfoManager.Models
{
    public class SpaceMember
    {
        public int Id { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }
        public int SpaceId { get; set; }
        public Space Space { get; set; }


    }
}
