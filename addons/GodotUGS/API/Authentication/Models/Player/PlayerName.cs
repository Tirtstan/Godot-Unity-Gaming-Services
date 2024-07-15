namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class PlayerName
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("autoGenerated")]
    public bool AutoGenerated { get; set; }
}