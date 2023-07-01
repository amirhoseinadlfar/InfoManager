using InfoManagerShared.Requests;

namespace InfoManager.Server.Controllers.Requests
{
    public class DeleteSpaceRequest : IDeleteSpaceRequest
    {
        public int SpaceId { get; set; }
    }
}
