namespace InfoManager.Shared.Requests;
public interface ISignUpRequest
{
    string Username { get; set; }
    string Name { get; set; }
    string Password { get; set; }
}