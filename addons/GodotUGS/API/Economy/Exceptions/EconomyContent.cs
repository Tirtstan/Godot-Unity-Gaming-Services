namespace Unity.Services.Economy.Models;

using System.Text.Json.Serialization;
using Unity.Services.Core.Models;

public class EconomyContent : CoreContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("instance")]
    public string Instance { get; set; }

    [JsonPropertyName("details")]
    public Detail[] Details { get; set; }
}
