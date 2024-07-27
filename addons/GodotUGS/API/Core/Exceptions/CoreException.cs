namespace Unity.Services.Core;

using System;
using Unity.Services.Core.Models;

public abstract class CoreException : Exception
{
    protected CoreException(string content, string message, Exception innerException)
        : base(message, innerException)
    {
        RawContent = content;
    }

    public abstract CoreContent Content { get; }
    public string RawContent { get; }
    public string Title => Content?.Title ?? string.Empty;
    public int Status => Content?.Status ?? 0;
    public override string Message => Content?.Detail ?? Content?.Detail2 ?? base.Message;
}
