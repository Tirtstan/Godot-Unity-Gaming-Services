using System.Text.Json.Serialization;
using Unity.Services.Core;

namespace Unity.Services.Leaderboards;

public class LeaderboardsContent : CoreContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("instance")]
    public string Instance { get; set; }
}
