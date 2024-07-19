namespace Unity.Services.Leaderboards;

using System.Text.Json.Serialization;
using Unity.Services.Core.Models;

public class LeaderboardsContent : CoreContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("instance")]
    public string Instance { get; set; }
}
