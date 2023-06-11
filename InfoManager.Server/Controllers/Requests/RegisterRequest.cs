using System.ComponentModel.DataAnnotations;

namespace InfoManager.Server.Controllers.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
