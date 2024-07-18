namespace Unity.Services.Economy;

public class EconomyPlayerInventory
{
    public string ProjectId { get; private set; }
    public string PlayerId { get; private set; }

    public EconomyPlayerInventory(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;
    }
}
