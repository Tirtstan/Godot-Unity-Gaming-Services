namespace Unity.Services.CloudSave.Internal;

public class DataService
{
    public DataService()
    {
        Player = new PlayerDataService();
        Custom = new CustomDataService();
    }

    public PlayerDataService Player { get; }
    public CustomDataService Custom { get; }
}
