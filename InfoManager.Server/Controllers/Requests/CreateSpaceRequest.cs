using InfoManager.Shared.Requests;

using System.ComponentModel.DataAnnotations;

namespace InfoManager.Server.Controllers.Requests
{
    public class CreateSpaceRequest : ICreateSpaceRequest
    {
        [StringLength(Models.Space.NameMaxLength,MinimumLength = Models.Space.NameMinLength)]
        public string Name { get; set; }
    }
}
