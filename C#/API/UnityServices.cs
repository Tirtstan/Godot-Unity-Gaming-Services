// Reference: https://services.docs.unity.com/docs/service-account-auth/

using System;
using Godot;
using RestSharp;

namespace Unity.Services.Core;

public partial class UnityServices : HttpRequest
{
    public static UnityServices Instance { get; private set; }
    private const string UnityServicesUrl = "https://services.api.unity.com";

    [Export(PropertyHint.ResourceType)]
    private APIResource apiResource;
    public string ProjectId => apiResource.ProjectId;
    public event Action<bool> OnInitialize;
    private RestClient restClient;

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        var restOptions = new RestClientOptions(UnityServicesUrl) { ThrowOnAnyError = true };
        restClient = new RestClient(restOptions);
    }

    public async void Initialize()
    {
        GD.Print("Initializing Unity Services");

        var request = new RestRequest(UnityServicesUrl).AddHeader(
            "Authorization",
            $"Basic {apiResource.ServiceAccountCredentials}"
        );

        var response = await restClient.ExecuteAsync(request);
        OnInitialize?.Invoke(response.IsSuccessful);
    }
}
