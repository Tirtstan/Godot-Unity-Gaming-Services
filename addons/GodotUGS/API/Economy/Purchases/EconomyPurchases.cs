namespace Unity.Services.Economy;

public class EconomyPurchases
{
    public string ProjectId { get; private set; }
    public string PlayerId { get; private set; }

    public EconomyPurchases(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;
    }
}
