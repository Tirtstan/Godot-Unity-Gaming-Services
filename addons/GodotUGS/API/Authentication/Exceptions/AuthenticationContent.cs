using System.Text.Json.Serialization;
using Unity.Services.Core;

namespace Unity.Services.Authentication;

public class AuthenticationContent : CoreContent
{
    [JsonPropertyName("details")]
    public Detail[] Details { get; set; }
}
