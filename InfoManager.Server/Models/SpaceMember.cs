namespace InfoManager.Server.Models
{
    public class SpaceMember
    {
        public int Id { get; set; }
        public SpaceMemberType Type { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }
        public int SpaceId { get; set; }
        public Space Space { get; set; }


    }
    public enum SpaceMemberType : byte
    {
        Owner = 0,
        Member = 1,
    }
}
