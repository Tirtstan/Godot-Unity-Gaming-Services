# Godot Unity Gaming Services

Basic SKD for connecting **[Unity Gaming Services](https://unity.com/solutions/gaming-services)** to **Godot 4.2+** using C#. Not sure if I will continue creating and updating this project as I just wanted the ability to use Unity's User Generated Content in any project I make.

**This SDK is still under development. It may never be finished.**

Maybe someone can use this as a jumping point to create a final version or contribute to this directly.

# Unity Gaming Services

## In Development

-   Authentication
    -   Support for session log ins
    -   Other informational methods (like getting the player's ID)

## Planned

-   Leaderboards
-   User Generated Content

# Architecture

Using the wonderful **[RestSharp](https://github.com/RestSharp/RestSharp)** to make this process a little easier.

I have tried to keep the implementation of the SDK as similar to Unity's version for their engine. I am very inexperienced with the making of REST API's so bare with me here (or fork/contribute!).

Scripts are communicated by singletons like in Unity. I only use one initial Godot Autoload to instantiated all services.

**Some services require your Unity organization's service account credentials. You can find out more information [here](https://services.docs.unity.com/docs/service-account-auth/).**

# Services

## Initialization

### Default

```csharp
using Unity.Services.Core;

public override async void _Ready()
{
	UnityServices.Instance.OnInitialize += OnInitialize;
	await UnityServices.Instance.InitializeAsync(); // this is required to do anything with UGS
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
using Unity.Services.Core;

public override async void _Ready()
{
    var options = new InitializationOptions();
    initializationOptions.SetEnvironmentName("experimental");

    await UnityServices.Instance.InitializeAsync(options);
}
```

## Authentication

### Anonymous

```csharp
// If a player has signed in previously with a session token stored on the device, they are signed back in regardless of if they're an anonymous player or not.
private async void SignInAnonymously()
{
    try
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        GD.Print("Signed in as!: " + AuthenticationService.Instance.PlayerId);
    }
    catch (System.Exception e)
    {
        GD.PrintErr(e);
    }
}
```

### Username & Password

```csharp
using Unity.Services.Authentication;

private async void SignUp(string username, string password)
{
    try
    {
        await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
        GD.Print("Signed up as!: " + AuthenticationService.Instance.PlayerId);
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
        GD.Print("Signed in as!: " + AuthenticationService.Instance.PlayerId);
    }
    catch (System.Exception e)
    {
        GD.PrintErr(e);
    }
}
```
