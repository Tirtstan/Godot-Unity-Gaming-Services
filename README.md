# Godot Unity Gaming Services

Basic SKD for connecting **[Unity Gaming Services (UGS)](https://unity.com/solutions/gaming-services)** to **Godot 4.2+** using C#. Not sure if I will continue creating and updating this project as I just wanted the ability to use Unity's User Generated Content in any project I make.

**This SDK is still under development. It may never be finished.**

Maybe someone can use this as a jumping point to create a final version or contribute to this directly.

# Architecture

Using the wonderful **[RestSharp](https://github.com/RestSharp/RestSharp)** to make this process a little easier.

I have tried to keep the implementation of the SDK as similar to Unity's version for their engine. I am very inexperienced with the making of REST API's so bare with me here (or fork/contribute!).

Scripts are communicated by singletons like in Unity. I use one initial Godot Autoload to instantiated all child services.

**Some services require your [Unity organization's service account credentials](https://services.docs.unity.com/docs/service-account-auth).**

# Unity Gaming Services

## In Development

-   Authentication
    -   Code Sign In
    -   JWT token validating (no idea)
    -   Polish
-   Cloud Save

## Planned

-   Leaderboards
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

### Anonymous

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
