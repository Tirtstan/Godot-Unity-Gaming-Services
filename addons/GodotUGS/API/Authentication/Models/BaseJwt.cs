namespace Unity.Services.Authentication.Internal.Models;

// sourced from Unity

using System;
using System.Text.Json.Serialization;

public class BaseJwt
{
    public BaseJwt() { }

    [JsonPropertyName("exp")]
    public int ExpirationTimeUnix { get; set; }

    [JsonPropertyName("iat")]
    public int IssuedAtTimeUnix { get; set; }

    [JsonPropertyName("nbf")]
    public int NotBeforeTimeUnix { get; set; }

    [JsonIgnore]
    public DateTime? ExpirationTime => ConvertTimestamp(ExpirationTimeUnix);

    [JsonIgnore]
    public DateTime? IssuedAtTime => ConvertTimestamp(IssuedAtTimeUnix);

    [JsonIgnore]
    public DateTime? NotBeforeTime => ConvertTimestamp(NotBeforeTimeUnix);

    protected DateTime? ConvertTimestamp(int timestamp)
    {
        if (timestamp != 0)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            return dateTimeOffset.DateTime;
        }

        return null;
    }
}
