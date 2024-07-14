namespace Unity.Services.Authentication;

public struct Notification
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string PlayerID { get; set; }
    public string CaseID { get; set; }
    public string ProjectID { get; set; }
    public string Message { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    public string DeletedAt { get; set; }
}
