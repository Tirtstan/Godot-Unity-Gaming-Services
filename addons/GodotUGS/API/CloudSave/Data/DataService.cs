namespace Unity.Services.CloudSave.Internal;

public class DataService
{
    public DataService(string projectId, string playerId)
    {
        Player = new PlayerDataService(projectId, playerId);
        Custom = new CustomDataService(projectId, playerId);
    }

    public PlayerDataService Player { get; }
    public CustomDataService Custom { get; }
}
