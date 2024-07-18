namespace Unity.Services.Economy;

public class EconomyPlayerBalances
{
    public string ProjectId { get; private set; }
    public string PlayerId { get; private set; }

    public EconomyPlayerBalances(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;
    }
}
