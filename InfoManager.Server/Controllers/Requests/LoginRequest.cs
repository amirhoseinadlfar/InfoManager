using InfoManager.Shared.Requests;

using System.ComponentModel.DataAnnotations;

namespace InfoManager.Server.Controllers.Requests
{
    public class LoginRequest : ILoginRequest
    {
        [StringLength(Models.User.UsernameMaxLength,MinimumLength =Models.User.UsernameMinLength)]
        public required string Username { get; set; }
        [StringLength(Models.User.PasswordMaxLength,MinimumLength =Models.User.PasswordMinLength)]
        public required string Password { get; set; }
    }
}
