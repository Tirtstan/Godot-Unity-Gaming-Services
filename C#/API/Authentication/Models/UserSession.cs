namespace Unity.Services.Authentication.Models;

public class UserSession
{
    public int expiresIn { get; set; }
    public string idToken { get; set; }
    public string sessionToken { get; set; }
    public User user { get; set; }
    public string userId { get; set; }

    public override string ToString() =>
        $"IdToken: {idToken}, SessionToken: {sessionToken}, UserId: {userId}, ExpiresIn: {expiresIn}, User: {user}";
}
