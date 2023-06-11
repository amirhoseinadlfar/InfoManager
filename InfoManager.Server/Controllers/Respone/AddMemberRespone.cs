namespace InfoManager.Server.Controllers.Respone
{
    public class AddMemberRespone
    {
        public AddMemberError AddMemberError { get; set; }
    }
    public enum AddMemberError
    {
        None = 0,
        SpaceNotFound = 1,
        YouAreNotSpaceMember = 2,
        YouNeedPermission = 3,
    }
}
