using System;
using Godot;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;

public partial class Sample : Node
{
    private Button initializeButton;
    private Button signInButton;
    private RichTextLabel playerInfoLabel;
    private TextEdit leaderboardIdInput;
    private TextEdit scoreInput;
    private Button addScoreButton;
    private Button getScoresButton;
    private RichTextLabel leaderboardInfoLabel;

    public override void _Ready()
    {
        initializeButton = GetNode<Button>("InitializeButton");
        signInButton = GetNode<Button>("SignInButton");
        playerInfoLabel = GetNode<RichTextLabel>("PlayerInfoLabel");

        leaderboardIdInput = GetNode<TextEdit>("LeaderboardIdInput");
        scoreInput = GetNode<TextEdit>("ScoreInput");
        addScoreButton = GetNode<Button>("AddScoreButton");
        getScoresButton = GetNode<Button>("GetScoresButton");
        leaderboardInfoLabel = GetNode<RichTextLabel>("LeaderboardInfoLabel");

        UnityServices.Instance.OnInitialize += OnInitialize;

        initializeButton.Pressed += OnInitializeButtonPressed;
        signInButton.Pressed += OnSignInButtonPressed;

        addScoreButton.Pressed += OnAddScoreButtonPressed;
        getScoresButton.Pressed += OnGetScoresButtonPressed;
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
        catch (AuthenticationException e)
        {
            GD.PrintErr(e);
            signInButton.Disabled = false;
        }
    }

    private async void OnAddScoreButtonPressed()
    {
        if (string.IsNullOrEmpty(leaderboardIdInput.Text))
            return;

        if (double.TryParse(scoreInput.Text, out double score))
        {
            try
            {
                addScoreButton.Disabled = true;
                await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardIdInput.Text, score);
                GD.Print("Score added!");
            }
            catch (LeaderboardsException e)
            {
                GD.PrintErr(e);
            }
        }

        addScoreButton.Disabled = false;
    }

    private async void OnGetScoresButtonPressed()
    {
        if (string.IsNullOrEmpty(leaderboardIdInput.Text))
            return;

        try
        {
            GD.Print("Getting scores...");

            getScoresButton.Disabled = true;
            var scores = await LeaderboardsService.Instance.GetScoresAsync(leaderboardIdInput.Text);

            if (scores.Results != null)
            {
                string output = "";
                foreach (var score in scores.Results)
                {
                    output += $"Player: {score.PlayerName} - {score.Score}\n";
                }

                leaderboardInfoLabel.Text = $"[center]{output}[/center]";
                GD.Print("Scores retrieved!");
            }
            else
            {
                GD.Print("No scores found!");
            }
        }
        catch (LeaderboardsException e)
        {
            GD.PrintErr(e);
        }

        getScoresButton.Disabled = false;
    }

    private void OnInitialize(bool isInitialized)
    {
        signInButton.Disabled = !isInitialized;
        addScoreButton.Disabled = !isInitialized;
        getScoresButton.Disabled = !isInitialized;
    }

    public override void _ExitTree()
    {
        UnityServices.Instance.OnInitialize -= OnInitialize;

        initializeButton.Pressed -= OnInitializeButtonPressed;
        signInButton.Pressed -= OnSignInButtonPressed;

        addScoreButton.Pressed -= OnAddScoreButtonPressed;
        getScoresButton.Pressed -= OnGetScoresButtonPressed;
    }
}
