using System;
using System.Text.Json;
using Unity.Services.Core;

namespace Unity.Services.Authentication;

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
