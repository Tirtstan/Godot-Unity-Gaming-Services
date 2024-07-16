namespace Unity.Services.CloudSave.Internal;

public class PlayerFilesService
{
    private string ProjectId { get; }
    private string PlayerId { get; }

    public PlayerFilesService(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;
    }
}
