namespace Unity.Services.Ugc;

using System.Text.Json.Serialization;
using Unity.Services.Core.Models;

public class UgcContent : CoreContent
{
    [JsonPropertyName("code")]
    public int Code;

    [JsonPropertyName("details")]
    public Detail[] Details;
}
