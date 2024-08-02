# Godot Unity Gaming Services

Basic SKD for connecting **[Unity Gaming Services (UGS)](https://unity.com/solutions/gaming-services)** to **Godot 4+ (C# Mono)** (All latest stables & 4.3 beta 3).

Feel free to use this as a jumping point to create a bigger, final version or contribute directly.

# Architecture

Using the wonderful **[RestSharp](https://github.com/RestSharp/RestSharp)** to make this process a little easier.

I have tried to keep the implementation of the SDK as similar to Unity's version for their engine as I can. I am inexperienced with using REST API's so bare with me here (or fork/contribute!).

Scripts are communicated by singletons like in Unity. I use one initial Godot Autoload to instantiated all child services.

# Setup

> [!IMPORTANT]  
> **In your Godot project, install RestSharp.**

```console
dotnet add package RestSharp
```

**To use GodotUGS, you have to provide your game's project ID. Here's how you can:**

-   In Your Browser:
    -   Go to **[Unity Gaming Services](https://cloud.unity.com/home)** and login or create an account.
    -   At the top, choose a project or create one.
    -   Go to the dashboard of the project (on the left side).
    -   Click on the Project settings button in the top right.
    -   Copy the project ID.
-   In Godot:
    -   Locate the example APIResource in **"res://addons/GodotUGS/Resources/APIResource_Example.tres"**.
    -   Fill in the project ID field.
    -   Locate the GodotUGS autoload in **"res://addons/GodotUGS/Autoloads/GodotUGS.tscn"** and open it.
    -   In the UnityServices node, provide the APIResource file through the inspector.

Done!

# Unity Gaming Services

## Supported

-   Authentication
    -   Anonymous/Session
    -   Username & Password
-   Leaderboards
-   Cloud Save
-   User Generated Content
-   Friends (Not thoroughly tested)

## Planned

-   External Authentication Providers (Apple, Google, etc)

# Services

## Examples

-   **[Initialization](#initialization)**

    -   [Default](#default)
    -   [Environment](#custom-environment)

-   **[Authentication](#authentication)**

    -   [Anonymous](#anonymous--session-sign-in)
    -   [Username & Password](#username--password)

-   **[Leaderboards](#leaderboards)**

    -   [Adding A Score](#adding-a-score)
    -   [Getting Scores](#getting-scores)

-   **[Cloud Save](#cloud-save)**

    -   [Saving Items](#saving-items)
    -   [Loading Items](#loading-items)

-   **[User Generated Content](#user-generated-content)**

    -   [Uploading Content](#uploading-content)
    -   [Getting Specific Content](#getting-specific-content)

## Initialization

### Default

```cs
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

```cs
using Godot;
using Unity.Services.Core;

public override async void _Ready()
{
    var options = new InitializationOptions(); // will default to "production"
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

> [!NOTE]  
> If a player has signed in previously with a session token stored on the device, they are signed back in regardless of if they're an anonymous player or not.

```cs
using Godot;
using Unity.Services.Authentication;

private async void SignInAnonymously()
{
    try
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        GD.Print("Signed in ID: " + AuthenticationService.Instance.PlayerId);
    }
    catch (AuthenticationException e)
    {
        GD.PrintErr(e);
    }
}
```

### Username & Password

```cs
using Godot;
using Unity.Services.Authentication;

private async void SignUp(string username, string password)
{
    try
    {
        await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
        GD.Print("Signed up ID: " + AuthenticationService.Instance.PlayerId);
    }
    catch (AuthenticationException e)
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
    catch (AuthenticationException e)
    {
        GD.PrintErr(e);
    }
}
```

## Leaderboards

### Adding a score

```cs
using Godot;
using Unity.Services.Leaderboards;

private async void AddPlayerScore(string leaderboardId, double score)
{
    try
    {
        await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);
        GD.Print("Score added!");
    }
    catch (LeaderboardsException e)
    {
        GD.PrintErr(e);
    }
}
```

### Getting scores

```cs
using Godot;
using Unity.Services.Leaderboards;

private async void GetScores(string leaderboardId)
{
    try
    {
        var scores = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId);
        GD.Print("Total Scores: " + scores.Total);
    }
    catch (LeaderboardsException e)
    {
        GD.PrintErr(e);
    }
}
```

## Cloud Save

### Saving Items

```cs
using Godot;
using Unity.Services.CloudSave;

private async void SaveItem()
{
    try
    {
        await CloudSaveService.Instance.Data.Player.SaveAsync(
            new Dictionary<string, object>()
            {
                { "coins", 1000 },
                { "gems", 100 },
                {
                    "items",
                    new List<Item>
                    {
                        new Item(1, "Sword", "A sharp sword", 10.5f),
                        new Item(2, "Shield", "A strong shield", 15.5f)
                    }
                }
            }
        );
    }
    catch (CloudSaveException e)
    {
        GD.PrintErr(e);
    }
}
```

### Loading Items

```cs
using Godot;
using Unity.Services.CloudSave;

private async void LoadItem()
{
    try
    {
        var data = await CloudSaveService.Instance.Data.Player.LoadAllAsync();
        foreach (var pair in data)
        {
            GD.Print(pair.Key + ": " + pair.Value);

            if (pair.Value.TryGetValueAs(out Item item))
                GD.Print(item.Description);
        }
    }
    catch (CloudSaveException e)
    {
        GD.PrintErr(e);
    }
}
```

## User Generated Content

### Uploading Content

```cs
using Godot;
using Unity.Services.Ugc;

private async void CreateContentAsync(string name, string description, string contentPath)
{
    try
    {
        byte[] contentBytes = FileAccess.GetFileAsBytes(contentPath); // Godot path (res:// or user://)

        var content = await UgcService.Instance.CreateContentAsync(
            new CreateContentArgs(name, description, contentBytes)
        );

        GD.Print("Content ID: " + content.Id + " was uploaded!");
    }
    catch (UgcException e)
    {
        GD.PrintErr(e);
    }
}
```

### Getting Specific Content

```cs
using Godot;
using Unity.Services.Ugc;

private async void GetContentAsync(string contentId)
{
    try
    {
        var content = await UgcService.Instance.GetContentAsync(
            new GetContentArgs(contentId) { DownloadContent = true, DownloadThumbnail = true }
        );

        using (var file = FileAccess.Open("res://contentDownloaded.json", FileAccess.ModeFlags.Write))
        {
            file.StoreBuffer(content.DownloadedContent);
        }

        using (var thumb = FileAccess.Open("res://thumbnailDownloaded.jpg", FileAccess.ModeFlags.Write))
        {
            thumb.StoreBuffer(content.DownloadedThumbnail);
        }

        GD.Print("Content ID: " + content.Id);
        GD.Print("Content Name: " + content.Name);
        GD.Print("Content Description: " + content.Description);
    }
    catch (UgcException e)
    {
        GD.PrintErr(e);
    }
}
```
