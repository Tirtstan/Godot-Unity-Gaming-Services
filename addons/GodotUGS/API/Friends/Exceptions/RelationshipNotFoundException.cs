using System;

namespace Unity.Services.Friends;

public class RelationshipNotFoundException : Exception
{
    public RelationshipNotFoundException() { }

    public RelationshipNotFoundException(string message)
        : base(message) { }

    public RelationshipNotFoundException(string message, Exception inner)
        : base(message, inner) { }
}
