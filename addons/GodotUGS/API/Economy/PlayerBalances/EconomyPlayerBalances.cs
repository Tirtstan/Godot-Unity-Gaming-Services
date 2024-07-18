namespace Unity.Services.Economy;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serializers.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Economy.Models;

public class EconomyPlayerBalances
{
    /// <summary>
    /// Fires when the SDK updates a player's balance. The called action will be passed the currency ID that was updated.
    /// Note that this will NOT fire for balance changes from elsewhere not in this instance of the SDK, for example other
    /// server-side updates or updates from other devices.
    /// </summary>
    public event Action<string> BalanceUpdated;

    private RestClient balancesClient;
    private const string BalancesURL = "https://economy.services.api.unity.com/v2";
    private string ProjectId { get; }
    private string PlayerId { get; }

    public EconomyPlayerBalances(string projectId, string playerId)
    {
        ProjectId = projectId;
        PlayerId = playerId;

        var options = new RestClientOptions(BalancesURL)
        {
            Authenticator = new JwtAuthenticator(AuthenticationService.Instance.AccessToken)
        };
        balancesClient = new RestClient(
            options,
            configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions { })
        );

        balancesClient.AddDefaultHeaders(UnityServices.Instance.DefaultHeaders);
    }
}
