namespace Unity.Services.Economy;

using System.Text.Json.Serialization;

public class Metadata
{
    [JsonPropertyName("configAssignmentHash")]
    public string ConfigAssignmentHash;
}
