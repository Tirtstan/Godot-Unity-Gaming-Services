using System;
using System.Text.Json;

namespace Unity.Services.Core;

/// <summary>
/// A base exception type for failed requests.
/// </summary>
public class AuthenticationException : Exception
{
    public AuthenticationException(string content)
    {
        try
        {
            Content = JsonSerializer.Deserialize<AuthenticationContent>(content);
        }
        catch { }
    }

    public AuthenticationException(string content, string message, Exception innerException)
        : base(message, innerException)
    {
        try
        {
            Content = JsonSerializer.Deserialize<AuthenticationContent>(content);
        }
        catch { }
    }

    public AuthenticationContent Content { get; }
    public string Title => Content?.Title ?? "";
    public int Status => Content?.Status ?? 0;
    public string Detail => Content?.Detail ?? "";
}
