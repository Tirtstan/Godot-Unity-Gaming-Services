namespace Unity.Services.Economy;

using System;
using System.Text.Json;
using Unity.Services.Core;
using Unity.Services.Economy.Models;

public class EconomyException : CoreException
{
    public EconomyException(string content, string message, Exception innerException)
        : base(content, message, innerException)
    {
        try
        {
            Content = JsonSerializer.Deserialize<EconomyContent>(content);
        }
        catch { }
    }

    public override EconomyContent Content { get; }
}
