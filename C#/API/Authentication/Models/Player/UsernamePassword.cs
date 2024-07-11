namespace Unity.Services.Authentication.Models;

public class UsernamePassword
{
    public string username { get; set; } = "";
    public string createdAt { get; set; } = "";
    public string lastLoginAt { get; set; } = "";
    public string passwordUpdatedAt { get; set; } = "";
}
