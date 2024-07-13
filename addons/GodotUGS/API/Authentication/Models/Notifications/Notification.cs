namespace Unity.Services.Authentication;

public struct Notification
{
    public string id { get; set; }
    public string type { get; set; }
    public string playerID { get; set; }
    public string caseID { get; set; }
    public string projectID { get; set; }
    public string message { get; set; }
    public string createdAt { get; set; }
    public string updatedAt { get; set; }
    public string deletedAt { get; set; }
}
