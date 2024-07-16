namespace Unity.Services.CloudSave.Internal;

public class FilesService
{
    public FilesService()
    {
        Player = new PlayerFilesService();
    }

    public PlayerFilesService Player { get; }
}
