namespace Unity.Services.Authentication.Models;

public class User
{
    public bool disabled { get; set; }
    public ExternalId[] externalIds { get; set; } = { };
    public string id { get; set; } = "";
    public string username { get; set; } = "";

    public override string ToString() =>
        $"Id: {id}, Username: {username}, Disabled: {disabled}, ExternalIds (Count): {externalIds.Length}";
}
