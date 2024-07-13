using System;
using Godot;
using Unity.Services.Authentication;
using Unity.Services.Core;

public partial class Sample : Node
{
    private Button initializeButton;
    private Button signInButton;
    private RichTextLabel playerInfoLabel;

    public override void _Ready()
    {
        initializeButton = GetNode<Button>("Buttons/InitializeButton");
        signInButton = GetNode<Button>("Buttons/SignInButton");
        playerInfoLabel = GetNode<RichTextLabel>("InformationLabels/PlayerInfoLabel");

        UnityServices.Instance.OnInitialize += OnInitialize;
        initializeButton.Pressed += OnInitializeButtonPressed;
        signInButton.Pressed += OnSignInButtonPressed;
    }

    private async void OnInitializeButtonPressed()
    {
        try
        {
            playerInfoLabel.Text = "";
            await UnityServices.Instance.InitializeAsync();

            playerInfoLabel.AppendText("[center]Unity Services initialized![/center]");
            initializeButton.Disabled = true;
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
            initializeButton.Disabled = false;
        }
    }

    private async void OnSignInButtonPressed()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            string playerInfo =
                $"\nSigned In ID: {AuthenticationService.Instance.PlayerId}\n"
                + $"Signed In Profile: {AuthenticationService.Instance.Profile}";

            playerInfoLabel.AppendText($"[center]{playerInfo}[/center]");
            signInButton.Disabled = true;
        }
        catch (Exception e)
        {
            GD.PrintErr(e);
            signInButton.Disabled = false;
        }
    }

    private void OnInitialize(bool isInitialized)
    {
        signInButton.Disabled = !isInitialized;
    }

    public override void _ExitTree()
    {
        UnityServices.Instance.OnInitialize -= OnInitialize;
        initializeButton.Pressed -= OnInitializeButtonPressed;
        signInButton.Pressed -= OnSignInButtonPressed;
    }
}
