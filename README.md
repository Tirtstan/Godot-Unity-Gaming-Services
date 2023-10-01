# Godot-Unity-REST-API-CSharp

Basic SKD for connecting Unity Gaming Services to **Godot 4.1+**. Not sure if I will continue creating and updating this project, just want the ability to have include User Generated Content in any project I make so I started this.

**This SDK is still under development. It may never be finished.**  

Hopefully someone can use this as a jump start to implement a final version.

# Unity Gaming Services

## In Development
- Authentication
    - Username & Password
        - Validation
        - Storing player ID token
    - Other information methods (like getting the player's ID)

## Planned
- Authentication
    - Anonymous Sign-In (just not entirely sure how to implement it)
- User Generated Content

# Architecture

I have tried to keep the implementation of the SDK as similar to Unity's version for their engine. I'm not experienced with API's (and only have about a year with C#) so bare with me here (or contribute!).

Scripts are communicated by singletons like in Unity. I only use one initial Godot Autoload to instantiated all services. Their aren't any asynchronous methods as I don't think HttpRequest nodes in Godot have any awaitable methods.

## Authentication

### Username & Password

<u>Example:</u>
```csharp
private void SignUp(string username, string password)
{
    Error error = AuthenticationService.Instance.SignUpWithUsernamePassword(username,password);

    GD.PrintErr(error);
}

private void SignIn(string username, string password)
{
    Error error = AuthenticationService.Instance.SignInWithUsernamePassword(username,password);

    GD.PrintErr(error);
}
```