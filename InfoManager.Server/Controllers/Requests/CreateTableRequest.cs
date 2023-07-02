
using InfoManager.Shared.Requests;

using System.ComponentModel.DataAnnotations;

namespace InfoManager.Server.Controllers.Requests
{
    public class CreateTableRequest : ICreateTableRequest
    {
        public int SpaceId { get; set; }
        [StringLength(Models.Table.NameMaxLength,MinimumLength =Models.Table.NameMinLength)]
        public string TableName { get; set; }
    }
}
