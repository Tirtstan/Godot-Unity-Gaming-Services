namespace Unity.Services.Authentication;

using Unity.Services.Authentication.Models;

public class PlayerInfo
{
    public string Username => usernamepassword.username;
    public bool disabled { get; set; }
    public ExternalId[] externalIds { get; set; } = { };
    public string id { get; set; } = "";
    public string createdAt { get; set; } = "";
    public string lastLoginAt { get; set; } = "";
    public UsernamePassword usernamepassword { get; set; } = new();
}
