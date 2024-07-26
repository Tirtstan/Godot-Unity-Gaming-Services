namespace Unity.Services.Authentication.Internal.Models;

using System.Text.Json.Serialization;

public class AccessToken : BaseJwt
{
    public AccessToken() { }

    [JsonPropertyName("aud")]
    public string[] Audience { get; set; }

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }

    // [JsonPropertyName("ext")]
    // public AccessTokenExtraClaims Extra;

    [JsonPropertyName("iss")]
    public string Issuer { get; set; }

    [JsonPropertyName("jti")]
    public string JwtId { get; set; }

    [JsonPropertyName("project_id")]
    public string ProjectId { get; set; }

    [JsonPropertyName("scp")]
    public string[] Scope { get; set; }

    [JsonPropertyName("sub")]
    public string Subject { get; set; }

    [JsonPropertyName("sign_in_provider")]
    public string SignInProvider { get; set; }

    public override string ToString() =>
        $"Audience: {Audience}, ClientId: {ClientId}, IssuedAtTime: {IssuedAtTime}, Issuer: {Issuer}, JwtId: {JwtId}, ProjectId: {ProjectId}, Scope: {Scope}, Subject: {Subject}, SignInProvider: {SignInProvider}, ExpirationTime: {ExpirationTime}";
}
