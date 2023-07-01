using InfoManagerShared.Requests;

namespace InfoManager.Server.Controllers.Requests;

public class DeleteMemberRequest : IDeleteMemberRequest
{
    public int SpaceId { get; set; }
    public int MemberId { get; set; }
}
