namespace Unity.Services.Friends.Internal.Models;

using System.Text.Json.Serialization;

public class InternalPresence
{
    [JsonPropertyName("availability")]
    public string Availability { get; set; }

    [JsonPropertyName("activity")]
    public object Activity { get; set; }
}
