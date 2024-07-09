namespace Unity.Services.Authentication.Models;

public class User
{
    public bool disabled { get; set; }
    public string[] externalIds { get; set; }
    public string id { get; set; }
    public string username { get; set; }

    public override string ToString() =>
        $"Id: {id}, Username: {username}, Disabled: {disabled}, ExternalIds: {string.Join(", ", externalIds)}";
}
