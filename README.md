# Godot-Unity-Gaming-Services

Basic SKD for connecting **[Unity Gaming Services](https://unity.com/solutions/gaming-services)** to **Godot 4.2+**. Not sure if I will continue creating and updating this project as I just wanted the ability to use Unity's User Generated Content in any project I make.

**This SDK is still under development. It may never be finished.**

Hopefully someone can use this as a jump start to implement a final version or guide me through it.

# Unity Gaming Services

## In Development

-   Authentication
    -   Username & Password
        -   Validation
        -   Storing player ID token
    -   Other informational methods (like getting the player's ID)

## Planned

-   Authentication
    -   Anonymous Sign-In (just not entirely sure how to implement it)
-   User Generated Content

# Architecture

Using the wonderful **[RestSharp](https://github.com/RestSharp/RestSharp)** to make this process a little easier.

I have tried to keep the implementation of the SDK as similar to Unity's version for their engine. I am very inexperienced with the making of REST API's so bare with me here (or fork/contribute!).

Scripts are communicated by singletons like in Unity. I only use one initial Godot Autoload to instantiated all services.

**Some services require your Unity organization's service account credentials. You can find out more information [here](https://services.docs.unity.com/docs/service-account-auth/).**

# Services

## Initialization

```csharp
public override void _Ready()
{
    UnityServices.Instance.OnInitialize += OnInitialize;
    UnityServices.Instance.Initialize();
}

private void OnInitialize(bool isInitialized)
{
    if (!isInitialized)
        return;

    GD.Print("Unity Services Initialized!);
}
```

## Authentication

### Username & Password

```csharp
private void SignUp(string username, string password)
{
    Error error = AuthenticationService.Instance.SignUpWithUsernamePassword(username, password);

    GD.PrintErr(error);
}

private void SignIn(string username, string password)
{
    Error error = AuthenticationService.Instance.SignInWithUsernamePassword(username, password);

    GD.PrintErr(error);
}
```
