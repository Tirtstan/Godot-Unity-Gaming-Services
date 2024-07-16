using System;

namespace Unity.Services.Core;

public abstract class CoreException : Exception
{
    protected CoreException(string content, string message, Exception innerException)
        : base(message, innerException) { }

    public abstract CoreContent Content { get; }
    public string Title => Content?.Title ?? string.Empty;
    public int Status => Content?.Status ?? 0;
    public override string Message => Content?.Detail ?? string.Empty;
}
