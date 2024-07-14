namespace Unity.Services.Authentication.Models;

public class UserSession
{
    public int ExpiresIn { get; set; }
    public string IdToken { get; set; } = "";
    public string SessionToken { get; set; } = "";
    public double LastNotificationDate { get; set; }
    public User User { get; set; } = new();
    public string UserId { get; set; } = "";

    public override string ToString() =>
        $"IdToken: {IdToken}, SessionToken: {SessionToken}, UserId: {UserId}, ExpiresIn: {ExpiresIn}, User: {User}";
}
