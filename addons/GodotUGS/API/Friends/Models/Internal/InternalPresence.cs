namespace Unity.Services.Friends.Internal.Models;

using System.Text.Json.Serialization;
using Unity.Services.Friends.Models;

public class InternalPresence
{
    [JsonPropertyName("availability")]
    public Availability Availability { get; set; }

    [JsonPropertyName("activity")]
    public object Activity { get; set; }
}
