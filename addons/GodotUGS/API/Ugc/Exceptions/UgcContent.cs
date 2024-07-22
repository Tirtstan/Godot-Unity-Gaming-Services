namespace Unity.Services.Ugc;

using System.Text.Json.Serialization;
using Unity.Services.Core.Models;

public class UgcContent : CoreContent
{
    [JsonPropertyName("details")]
    public Detail[] details;
}
