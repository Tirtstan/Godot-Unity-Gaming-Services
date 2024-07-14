namespace Unity.Services.Authentication;

using System.Text.Json.Serialization;

public struct Notification
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("playerID")]
    public string PlayerId { get; set; }

    [JsonPropertyName("caseID")]
    public string CaseId { get; set; }

    [JsonPropertyName("projectID")]
    public string ProjectId { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("createdAt")]
    public string CreatedAt { get; set; }

    [JsonPropertyName("updatedAt")]
    public string UpdatedAt { get; set; }

    [JsonPropertyName("deletedAt")]
    public string DeletedAt { get; set; }
}
