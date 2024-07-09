// References: https://services.docs.unity.com/docs/service-account-auth/ && https://restsharp.dev/docs/usage/basics

using System;
using System.Threading.Tasks;
using Godot;
using RestSharp;

namespace Unity.Services.Core;

public partial class UnityServices : Node
{
    [Export(PropertyHint.ResourceType)]
    private APIResource apiResource;
    public static UnityServices Instance { get; private set; }
    private RestClient restClient;
    private const string UnityServicesUrl = "https://services.api.unity.com";
    public string ProjectId => apiResource.ProjectId;
    public event Action<bool> OnInitialize;

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        restClient = new RestClient(UnityServicesUrl);
    }

    public async Task InitializeAsync()
    {
        var request = new RestRequest(UnityServicesUrl).AddHeader(
            "Authorization",
            $"Basic {apiResource.ServiceAccountCredentials}"
        );

        var response = await restClient.ExecuteAsync(request);
        OnInitialize?.Invoke(response.IsSuccessful);

        if (!response.IsSuccessful)
            throw response.ErrorException;
    }
}
