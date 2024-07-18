namespace Unity.Services.Authentication;

using System;
using System.Text.Json;
using Unity.Services.Authentication.Models;
using Unity.Services.Core;

/// <summary>
/// AuthenticationException represents a runtime exception from authentication.
/// </summary>
public class AuthenticationException : CoreException
{
    public AuthenticationException(string content, string message, Exception innerException)
        : base(content, message, innerException)
    {
        try
        {
            Content = JsonSerializer.Deserialize<AuthenticationContent>(content);
        }
        catch { }
    }

    public override AuthenticationContent Content { get; }
}
