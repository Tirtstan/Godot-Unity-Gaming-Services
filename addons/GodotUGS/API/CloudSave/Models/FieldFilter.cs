namespace Unity.Services.CloudSave.Models;

// sourced from Unity

using System.Text.Json.Serialization;

/// <summary>
/// A field filter for querying an index
/// </summary>
public class FieldFilter
{
    /// <summary>
    /// A field filter for querying an index
    /// </summary>
    /// <param name="key">Item key</param>
    /// <param name="value">The indexed Cloud Save value</param>
    /// <param name="op">The comparison operator to use for the filter. The specified value is compared to the indexed value (lexicographically for string data, numerically for numerical data) using one of the following operators: * &#x60;EQ&#x60; - Equal * &#x60;NE&#x60; - Not Equal * &#x60;LT&#x60; - Less Than * &#x60;LE&#x60; - Less Than or Equal * &#x60;GT&#x60; - Greater Than * &#x60;GE&#x60; - Greater Than or Equal</param>
    /// <param name="asc">Whether the field is sorted in ascending order</param>
    public FieldFilter(string key, object value, OpOptions op, bool asc)
    {
        Key = key;
        Value = value;
        Op = op;
        Asc = asc;
    }

    /// <summary>
    /// Item key
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; }

    /// <summary>
    /// The indexed Cloud Save value
    /// </summary>
    [JsonPropertyName("value")]
    public object Value { get; }

    /// <summary>
    /// The comparison operator to use for the filter. The specified value is compared to the indexed value (lexicographically for string data, numerically for numerical data) using one of the following operators: * &#x60;EQ&#x60; - Equal * &#x60;NE&#x60; - Not Equal * &#x60;LT&#x60; - Less Than * &#x60;LE&#x60; - Less Than or Equal * &#x60;GT&#x60; - Greater Than * &#x60;GE&#x60; - Greater Than or Equal
    /// </summary>
    [JsonPropertyName("op")]
    public OpOptions Op { get; }

    /// <summary>
    /// Whether the field is sorted in ascending order
    /// </summary>
    [JsonPropertyName("asc")]
    public bool Asc { get; }

    /// <summary>
    /// The comparison operator to use for the filter. The specified value is compared to the indexed value (lexicographically for string data, numerically for numerical data) using one of the following operators: * &#x60;EQ&#x60; - Equal * &#x60;NE&#x60; - Not Equal * &#x60;LT&#x60; - Less Than * &#x60;LE&#x60; - Less Than or Equal * &#x60;GT&#x60; - Greater Than * &#x60;GE&#x60; - Greater Than or Equal
    /// </summary>
    /// <value>The comparison operator to use for the filter. The specified value is compared to the indexed value (lexicographically for string data, numerically for numerical data) using one of the following operators: * &#x60;EQ&#x60; - Equal * &#x60;NE&#x60; - Not Equal * &#x60;LT&#x60; - Less Than * &#x60;LE&#x60; - Less Than or Equal * &#x60;GT&#x60; - Greater Than * &#x60;GE&#x60; - Greater Than or Equal</value>
    public class OpOptions
    {
        public static readonly string EQ = "EQ";
        public static readonly string NE = "NE";
        public static readonly string LT = "LT";
        public static readonly string LE = "LE";
        public static readonly string GT = "GT";
        public static readonly string GE = "GE";
    }
    // public enum OpOptions
    // {
    //     /// <summary>
    //     /// Enum EQ for value: EQ
    //     /// </summary>
    //     [JsonPropertyName("EQ")]
    //     EQ = 1,

    //     /// <summary>
    //     /// Enum NE for value: NE
    //     /// </summary>
    //     [JsonPropertyName("NE")]
    //     NE = 2,

    //     /// <summary>
    //     /// Enum LT for value: LT
    //     /// </summary>
    //     [JsonPropertyName("LT")]
    //     LT = 3,

    //     /// <summary>
    //     /// Enum LE for value: LE
    //     /// </summary>
    //     [JsonPropertyName("LE")]
    //     LE = 4,

    //     /// <summary>
    //     /// Enum GT for value: GT
    //     /// </summary>
    //     [JsonPropertyName("GT")]
    //     GT = 5,

    //     /// <summary>
    //     /// Enum GE for value: GE
    //     /// </summary>
    //     [JsonPropertyName("GE")]
    //     GE = 6
    // }
}
