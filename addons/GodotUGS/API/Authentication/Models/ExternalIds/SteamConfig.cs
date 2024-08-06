namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;

public class SteamConfig
{
    /// <summary>
    /// Constructor
    /// </summary>
    internal SteamConfig() { }

    /// <summary>
    /// Steam identity field to identify the calling service.
    /// </summary>
    [JsonPropertyName("identity")]
    public string identity;

    /// <summary>
    /// App Id that was used to generate the ticket. Only required for additional app ids (e.g.: PlayTest, Demo, etc)
    /// </summary>
    [JsonPropertyName("appId")]
    public string appId;
}
