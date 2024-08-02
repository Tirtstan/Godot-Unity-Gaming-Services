namespace Unity.Services.Authentication.Internal;

// sourced from Unity

using System;
using System.Text;
using System.Text.Json;
using Unity.Services.Authentication.Internal.Models;

/// <summary>
/// Trimmed-down and specialized version of:
/// https://github.com/monry/JWT-for-Unity/blob/master/JWT/JWT.cs
/// At time of writing, this source was public domain (Creative Commons 0)
/// </summary>
public class JwtDecoder
{
    private static readonly char[] k_JwtSeparator = { '.' };

    public static T Decode<T>(string token)
        where T : BaseJwt
    {
        var parts = token.Split(k_JwtSeparator);
        if (parts.Length == 3)
        {
            var payload = parts[1];
            var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            var payloadData = JsonSerializer.Deserialize<T>(payloadJson);

            return payloadData;
        }

        return null;
    }

    private static byte[] Base64UrlDecode(string input)
    {
        var output = input;
        output = output.Replace('-', '+'); // 62nd char of encoding
        output = output.Replace('_', '/'); // 63rd char of encoding

        var mod4 = input.Length % 4;
        if (mod4 > 0)
        {
            output += new string('=', 4 - mod4);
        }

        var converted = Convert.FromBase64String(output); // Standard base64 decoder
        return converted;
    }
}
