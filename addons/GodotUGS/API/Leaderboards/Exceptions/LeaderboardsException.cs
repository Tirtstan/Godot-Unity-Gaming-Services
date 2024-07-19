namespace Unity.Services.Leaderboards;

using System;
using System.Text.Json;
using Unity.Services.Core;

/// <summary>
/// LeaderboardsException represents a runtime exception from Leaderboards.
/// </summary>
public class LeaderboardsException : CoreException
{
    public LeaderboardsException(string content, string message, Exception innerException)
        : base(content, message, innerException)
    {
        try
        {
            Content = JsonSerializer.Deserialize<LeaderboardsContent>(content);
        }
        catch { }
    }

    public override LeaderboardsContent Content { get; }
}
