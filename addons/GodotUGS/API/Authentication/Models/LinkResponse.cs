namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class LinkResponse
{
    public LinkResponse() { }

    [JsonPropertyName("user")]
    public User User;
}
