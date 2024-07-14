namespace Unity.Services.Authentication.Models;

public class User
{
    public bool Disabled { get; set; }
    public ExternalId[] ExternalIds { get; set; } = { };
    public string Id { get; set; } = "";
    public string Username { get; set; } = "";

    public override string ToString() =>
        $"Id: {Id}, Username: {Username}, Disabled: {Disabled}, ExternalIds (Count): {ExternalIds.Length}";
}
