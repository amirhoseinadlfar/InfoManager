using InfoManagerShared.Requests;

using System.ComponentModel.DataAnnotations;

namespace InfoManager.Server.Controllers.Requests
{
    public class EditSpaceRequest : IEditSpaceRequest
    {
        public int SpaceId { get; set; }

        [StringLength(Models.Space.NameMaxLength,MinimumLength =Models.Space.NameMinLength)]
        public string? Name { get; set; }
    }
}
