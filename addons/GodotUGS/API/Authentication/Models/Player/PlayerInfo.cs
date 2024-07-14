namespace Unity.Services.Authentication;

using Unity.Services.Authentication.Models;

public class PlayerInfo
{
    public string Username => UsernamePassword.Username;
    public bool Disabled { get; set; }
    public Identity[] ExternalIds { get; set; } = { };
    public string Id { get; set; } = "";
    public string CreatedAt { get; set; } = "";
    public string LastLoginAt { get; set; } = "";
    public UsernameInfo UsernamePassword { get; set; } = new();
}
