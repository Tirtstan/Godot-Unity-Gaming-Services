namespace Unity.Services.CloudSave.Internal;

public class CustomDataService
{
    private string ProjectId { get; }
    private string PlayerId { get; }

    public CustomDataService(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;
    }
}
