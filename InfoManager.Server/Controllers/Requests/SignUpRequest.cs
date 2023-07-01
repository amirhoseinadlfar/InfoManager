using InfoManager.Shared.Requests;

using System.ComponentModel.DataAnnotations;

namespace InfoManager.Server.Controllers.Requests;

public class SignUpRequest : ISignUpRequest
{

    [StringLength(Models.User.UsernameMaxLength,MinimumLength =Models.User.UsernameMinLength)]
    public required string Username { get; set; }
    [StringLength(Models.User.NameMaxLength,MinimumLength =Models.User.NameMinLength)]
    public required string Name { get; set; }
    [StringLength(Models.User.PasswordMaxLength,MinimumLength =Models.User.PasswordMinLength)]
    public required string Password { get; set; }
}
