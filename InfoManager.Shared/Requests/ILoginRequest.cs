namespace InfoManager.Shared.Requests;

public interface ILoginRequest
{
    string Username { get; set; }
    string Password { get; set; }
}