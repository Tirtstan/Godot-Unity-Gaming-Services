namespace Unity.Services.CloudSave;

using System;
using System.Text.Json;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;

public class CloudSaveException : CoreException
{
    public CloudSaveException(string content, string message, Exception innerException)
        : base(content, message, innerException)
    {
        try
        {
            Content = JsonSerializer.Deserialize<CloudSaveContent>(content);
        }
        catch { }
    }

    public override CloudSaveContent Content { get; }
}
