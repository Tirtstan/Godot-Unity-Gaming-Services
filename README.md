# Godot Unity Gaming Services

Basic SKD for connecting **[Unity Gaming Services (UGS)](https://unity.com/solutions/gaming-services)** to **Godot 4.2+** using C#.

**This SDK is still under early development.**

Feel free to use this as a jumping point to create a bigger, final version or contribute directly.

# Architecture

Using the wonderful **[RestSharp](https://github.com/RestSharp/RestSharp)** to make this process a little easier.

I have tried to keep the implementation of the SDK as similar to Unity's version for their engine. I am very inexperienced with the making of REST API's so bare with me here (or fork/contribute!).

Scripts are communicated by singletons like in Unity. I use one initial Godot Autoload to instantiated all child services.

# Setup

**In your Godot project, install RestSharp.**

```console
dotnet add package RestSharp
```

**To use GodotUGS, you have to provide your game's project ID. Here's how you can:**

-   In Your Browser:
    -   Go to the [UGS Website](https://cloud.unity.com/home) and login or create an account.
    -   At the top, choose a project or create one.
    -   Go to the dashboard of the project (on the side).
    -   Click on the Project settings button in the top right.
    -   Copy the project ID.
-   In Godot:
    -   Locate the example APIResource in **"res://addons/GodotUGS/Resources/APIResource.tres.example"**.
    -   Remove the ".example" extension.
    -   Fill in the project ID field.
    -   Locate the GodotUGS autoload in **"res://addons/GodotUGS/Autoloads/GodotUGS.tscn"**.
    -   In the UnityServices node, provide the APIResource.tres through the inspector.

Done!

# Unity Gaming Services

## Supported

-   Authentication
    -   Anonymous/Session
    -   Username & Password
-   Leaderboards

## Planned

-   Authentication
    -   JWT token validating & refreshing (no idea)
-   Cloud Save
-   User Generated Content

# Services

## Initialization

### Default

```csharp
using Godot;
using Unity.Services.Core;

public override async void _Ready()
{
    UnityServices.Instance.OnInitialize += OnInitialize;

    try
    {
        await UnityServices.Instance.InitializeAsync(); // required to do anything with UGS
    }
    catch (System.Exception e)
    {
        GD.PrintErr(e);
    }
}

private void OnInitialize(bool isInitialized)
{
    if (!isInitialized)
        return;

    GD.Print("Unity Services Initialized!");
}
```

### Custom Environment

```csharp
using Godot;
using Unity.Services.Core;

public override async void _Ready()
{
    var options = new InitializationOptions();
    initializationOptions.SetEnvironmentName("experimental");

    try
    {
        await UnityServices.Instance.InitializeAsync(options);
    }
    catch (System.Exception e)
    {
        GD.PrintErr(e);
    }
}
```

## Authentication

### Anonymous & Session Sign In

```csharp
using Godot;
using Unity.Services.Authentication;

// If a player has signed in previously with a session token stored on the device,
// they are signed back in regardless of if they're an anonymous player or not.
private async void SignInAnonymously()
{
    try
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        GD.Print("Signed in ID: " + AuthenticationService.Instance.PlayerId);
    }
    catch (System.Exception e)
    {
        GD.PrintErr(e);
    }
}
```

### Username & Password

```csharp
using Godot;
using Unity.Services.Authentication;

private async void SignUp(string username, string password)
{
    try
    {
        await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
        GD.Print("Signed up ID: " + AuthenticationService.Instance.PlayerId);
    }
    catch (System.Exception e)
    {
        GD.PrintErr(e);
    }
}

private async void SignIn(string username, string password)
{
    try
    {
        await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
        GD.Print("Signed in ID: " + AuthenticationService.Instance.PlayerId);
    }
    catch (System.Exception e)
    {
        GD.PrintErr(e);
    }
}
```

## Leaderboards

### Adding a score

```csharp
using Godot;
using Unity.Services.Leaderboards;

private async void AddPlayerScore(string leaderboardId, double score)
{
    try
    {
        await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
        GD.Print("Score added!");
    }
    catch (System.Exception e)
    {
        GD.PrintErr(e);
    }
}
```

### Getting scores

```csharp
using Godot;
using Unity.Services.Leaderboards;

private async void GetScores(string leaderboardId)
{
    try
    {
        var scores = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
        GD.Print("Total Scores: " + scores.Total);
    }
    catch (System.Exception e)
    {
        GD.PrintErr(e);
    }
}
```
