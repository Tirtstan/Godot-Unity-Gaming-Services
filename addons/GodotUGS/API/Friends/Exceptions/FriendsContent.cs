namespace Unity.Services.Friends.Models;

using System.Text.Json.Serialization;
using Unity.Services.Core.Models;

public class FriendsContent : CoreContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("requestID")]
    public string RequestId { get; set; }
}
