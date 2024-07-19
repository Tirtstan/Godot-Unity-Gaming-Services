namespace Unity.Services.Friends;

using System;
using System.Text.Json;
using Unity.Services.Core;
using Unity.Services.Friends.Models;

public class FriendsServiceException : CoreException
{
    public FriendsServiceException(string content, string message, Exception innerException)
        : base(content, message, innerException)
    {
        try
        {
            Content = JsonSerializer.Deserialize<FriendsContent>(content);
        }
        catch { }
    }

    public override FriendsContent Content { get; }
}
