namespace Unity.Services.Authentication.Models;

public class UsernamePassword
{
    public string Username { get; set; } = "";
    public string CreatedAt { get; set; } = "";
    public string LastLoginAt { get; set; } = "";
    public string PasswordUpdatedAt { get; set; } = "";
}
