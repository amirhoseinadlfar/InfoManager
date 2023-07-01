using InfoManager.Shared.Requests;

namespace InfoManager.Server.Controllers.Requests
{
    public class AddSpaceMemberRequest : IAddSpaceMemberRequest
    {
        public int SpaceId { get; set; }
    }
}
