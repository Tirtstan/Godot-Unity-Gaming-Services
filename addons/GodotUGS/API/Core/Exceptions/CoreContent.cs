using System.Text.Json.Serialization;

namespace Unity.Services.Core;

public class CoreContent
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("detail")]
    public string Detail { get; set; }
}
