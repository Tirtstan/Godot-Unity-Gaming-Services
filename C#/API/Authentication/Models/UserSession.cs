namespace Unity.Services.Authentication.Models;

public class UserSession
{
    public UserSession(int expiresIn, string idToken, string sessionToken, User user, string userId)
    {
        this.expiresIn = expiresIn;
        this.idToken = idToken;
        this.sessionToken = sessionToken;
        this.user = user;
        this.userId = userId;
    }

    public int expiresIn { get; set; }
    public string idToken { get; set; }
    public string sessionToken { get; set; }
    public User user { get; set; }
    public string userId { get; set; }

    public override string ToString() =>
        $"IdToken: {idToken}, SessionToken: {sessionToken}, UserId: {userId}, ExpiresIn: {expiresIn}, User: {user}";
}
