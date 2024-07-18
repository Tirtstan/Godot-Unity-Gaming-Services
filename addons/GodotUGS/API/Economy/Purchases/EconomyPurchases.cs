namespace Unity.Services.Economy;

public class EconomyPurchases
{
    private string ProjectId { get; }
    private string PlayerId { get; }

    public EconomyPurchases(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;
    }
}
