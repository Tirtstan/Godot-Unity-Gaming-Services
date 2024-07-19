namespace Unity.Services.CloudSave.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;
using Unity.Services.Core.Models;

public class CloudSaveContent : CoreContent
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("instance")]
    public string Instance { get; set; }

    [JsonPropertyName("errors")]
    public List<Error> Errors { get; set; }
}
