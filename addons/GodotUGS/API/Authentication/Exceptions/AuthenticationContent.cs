namespace Unity.Services.Authentication.Models;

using System.Text.Json.Serialization;
using Unity.Services.Core.Models;

public class AuthenticationContent : CoreContent
{
    [JsonPropertyName("details")]
    public Detail[] Details { get; set; }
}
