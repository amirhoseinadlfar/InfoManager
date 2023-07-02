using InfoManagerShared.Requests;

namespace InfoManager.Server.Controllers.Requests
{
    public class GetSpaceTablesRequest : IGetSpaceTablesRequest
    {
        public int SpaceId { get; set; }
    }
}
