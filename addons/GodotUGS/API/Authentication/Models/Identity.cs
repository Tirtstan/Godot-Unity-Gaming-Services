namespace Unity.Services.Authentication.Models;

// sourced from Unity

using Unity.Services.Authentication.Internal.Models;

public class Identity
{
    /// <summary>
    /// The identity type id.
    /// </summary>
    public string TypeId;

    /// <summary>
    /// The identity user id
    /// </summary>
    public string UserId;

    /// <summary>
    /// Constructor
    /// </summary>
    internal Identity(ExternalIdentity externalIdentity)
    {
        if (externalIdentity != null)
        {
            TypeId = externalIdentity.ProviderId;
            UserId = externalIdentity.ExternalId;
        }
    }
}
