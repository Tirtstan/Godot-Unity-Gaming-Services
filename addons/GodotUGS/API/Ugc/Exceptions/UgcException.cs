namespace Unity.Services.Ugc;

using System;
using System.Text.Json;
using Unity.Services.Core;

public class UgcException : CoreException
{
    public UgcException(string content, string message, Exception innerException)
        : base(content, message, innerException)
    {
        try
        {
            Content = JsonSerializer.Deserialize<UgcContent>(content);
        }
        catch { }
    }

    public override UgcContent Content { get; }
}
