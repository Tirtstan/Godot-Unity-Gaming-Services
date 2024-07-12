using Godot;
using Unity.Services.Authentication;
using Unity.Services.Core;

public partial class Test : Node
{
    public override async void _Ready()
    {
        UnityServices.Instance.OnInitialize += OnInitialize;

        try
        {
            await UnityServices.Instance.InitializeAsync();
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private async void OnInitialize(bool isInitialized)
    {
        if (!isInitialized)
            return;

        GD.Print("Unity Services initialized!");

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            PrintUser();
        }
        catch (System.Exception e)
        {
            GD.PrintErr(e);
        }
    }

    private void PrintUser()
    {
        GD.Print("Signed In ID: " + AuthenticationService.Instance.PlayerId);
        GD.Print("Signed In Name: " + AuthenticationService.Instance.PlayerName);
        GD.Print("Signed In Profile: " + AuthenticationService.Instance.Profile);
    }

    public override void _ExitTree()
    {
        UnityServices.Instance.OnInitialize -= OnInitialize;
    }
}
