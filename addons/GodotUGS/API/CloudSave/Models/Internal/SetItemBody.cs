namespace Unity.Services.CloudSave.Internal.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class SetItemBody
{
    public SetItemBody(string key, object value, string writeLock = default)
    {
        Key = key;
        Value = value;
        WriteLock = writeLock;
    }

    [JsonPropertyName("key")]
    public string Key { get; }

    [JsonPropertyName("value")]
    public object Value { get; }

    [JsonPropertyName("writeLock")]
    public string WriteLock { get; }
}

public class SetItemBatchBody
{
    public SetItemBatchBody(List<SetItemBody> data = default)
    {
        Data = data;
    }

    [JsonPropertyName("data")]
    public List<SetItemBody> Data { get; }
}
