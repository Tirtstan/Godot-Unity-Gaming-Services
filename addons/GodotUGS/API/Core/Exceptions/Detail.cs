namespace Unity.Services.Core.Models;

using System.Text.Json.Serialization;

public class Detail
{
    [JsonPropertyName("code")]
    public string ErrorCode { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
