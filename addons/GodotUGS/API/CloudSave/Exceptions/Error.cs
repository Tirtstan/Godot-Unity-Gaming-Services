namespace Unity.Services.CloudSave.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Error
{
    [JsonPropertyName("field")]
    public string Field { get; set; }

    [JsonPropertyName("messages")]
    public List<string> Messages { get; set; }
}
