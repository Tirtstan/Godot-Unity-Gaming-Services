namespace Unity.Services.Ugc.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Reason enum.
/// </summary>
/// <value></value>
public class Reason
{
    [JsonPropertyName("dmcaRequest")]
    public static readonly string DmcaRequest = "dmcaRequest";

    [JsonPropertyName("nonFunctional")]
    public static readonly string NonFunctional = "nonFunctional";

    [JsonPropertyName("inappropriate")]
    public static readonly string Inappropriate = "inappropriate";

    [JsonPropertyName("illegalOrStolen")]
    public static readonly string IllegalOrStolen = "illegalOrStolen";

    [JsonPropertyName("misleading")]
    public static readonly string Misleading = "misleading";

    [JsonPropertyName("other")]
    public static readonly string Other = "other";
}

// public enum Reason
// {
//     /// <summary>
//     /// Enum DmcaRequest for value: dmcaRequest
//     /// </summary>
//     [EnumMember(Value = "dmcaRequest")]
//     DmcaRequest = 1,

//     /// <summary>
//     /// Enum NonFunctional for value: nonFunctional
//     /// </summary>
//     [EnumMember(Value = "nonFunctional")]
//     NonFunctional = 2,

//     /// <summary>
//     /// Enum Inappropriate for value: inappropriate
//     /// </summary>
//     [EnumMember(Value = "inappropriate")]
//     Inappropriate = 3,

//     /// <summary>
//     /// Enum IllegalOrStolen for value: illegalOrStolen
//     /// </summary>
//     [EnumMember(Value = "illegalOrStolen")]
//     IllegalOrStolen = 4,

//     /// <summary>
//     /// Enum Misleading for value: misleading
//     /// </summary>
//     [EnumMember(Value = "misleading")]
//     Misleading = 5,

//     /// <summary>
//     /// Enum Other for value: other
//     /// </summary>
//     [EnumMember(Value = "other")]
//     Other = 6
// }
