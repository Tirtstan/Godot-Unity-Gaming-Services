namespace Unity.Services.Economy;

public class EconomyPlayerInventory
{
    private string ProjectId { get; }
    private string PlayerId { get; }

    public EconomyPlayerInventory(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;
    }
}
