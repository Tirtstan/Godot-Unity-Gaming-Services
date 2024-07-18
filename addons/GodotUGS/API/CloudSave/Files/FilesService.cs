namespace Unity.Services.CloudSave.Internal;

public class FilesService
{
    public FilesService(string projectId, string playerId)
    {
        Player = new PlayerFilesService(projectId, playerId);
    }

    public PlayerFilesService Player { get; }
}
