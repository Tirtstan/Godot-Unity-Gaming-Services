namespace Unity.Services.Economy;

using Godot;
using Unity.Services.Authentication;
using Unity.Services.Core;

public partial class EconomyService : Node
{
    public static EconomyService Instance { get; private set; }
    public EconomyConfiguration Configuration { get; private set; }
    public EconomyPlayerBalances PlayerBalances { get; private set; }
    public EconomyPlayerInventory PlayerInventory { get; private set; }
    public EconomyPurchases Purchases { get; private set; }

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        AuthenticationService.Instance.SignedIn += OnSignedIn;
    }

    private void OnSignedIn()
    {
        Configuration = new EconomyConfiguration(
            UnityServices.Instance.ProjectId,
            AuthenticationService.Instance.PlayerId
        );

        PlayerBalances = new EconomyPlayerBalances(
            UnityServices.Instance.ProjectId,
            AuthenticationService.Instance.PlayerId
        );

        PlayerInventory = new EconomyPlayerInventory(
            UnityServices.Instance.ProjectId,
            AuthenticationService.Instance.PlayerId
        );

        Purchases = new EconomyPurchases(UnityServices.Instance.ProjectId, AuthenticationService.Instance.PlayerId);
    }

    public override void _ExitTree()
    {
        AuthenticationService.Instance.SignedIn -= OnSignedIn;
    }
}
