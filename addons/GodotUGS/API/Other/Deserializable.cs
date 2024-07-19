namespace Unity.Services;

using System.Text.Json;

/// <summary>
/// IDeserializable is an interface for wrapping generic objects that might
/// be returned as part of HTTP requests.
/// </summary>
public class Deserializable
{
    public Deserializable(string json)
    {
        RawJson = json;
    }

    public string RawJson { get; }

    /// <summary>
    /// Gets this object as the given type.
    /// </summary>
    /// <typeparam name="T">The type you want to convert this object to.</typeparam>
    /// <param name="options">The options to configure when deserializing.</param>
    /// <returns>If the object deserialized.</returns>
    public bool TryGetAs<T>(out T value, JsonSerializerOptions options = null)
    {
        try
        {
            value = JsonSerializer.Deserialize<T>(RawJson, options);
            return true;
        }
        catch { }

        value = default;
        return false;
    }
}
