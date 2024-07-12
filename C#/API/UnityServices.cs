// References: https://services.docs.unity.com/docs/service-account-auth/ && https://restsharp.dev/docs/usage/basics

using System;
using System.Threading.Tasks;
using Godot;
using RestSharp;

namespace Unity.Services.Core;

public partial class UnityServices : Node
{
    public static UnityServices Instance { get; private set; }
    public string Environment => initializationOptions.Environment;
    public string ProjectId => apiResource.ProjectId;
    public event Action<bool> OnInitialize;

    private const string UnityServicesUrl = "https://services.api.unity.com";
    private InitializationOptions initializationOptions = new();

    [Export(PropertyHint.ResourceType)]
    private APIResource apiResource;
    private RestClient restClient;

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        restClient = new RestClient(UnityServicesUrl);
    }

    public async Task InitializeAsync(InitializationOptions options = null)
    {
        if (apiResource == null)
            throw new NullReferenceException("API Resource is not set!");

        if (options != null)
            initializationOptions = options;

        var request = new RestRequest();
        if (!string.IsNullOrEmpty(apiResource.ServiceAccountCredentials))
            request.AddHeader("Authorization", $"Basic {apiResource.ServiceAccountCredentials}");

        var response = await restClient.ExecuteAsync(request);
        OnInitialize?.Invoke(response.IsSuccessful);

        if (!response.IsSuccessful)
            throw response.ErrorException;
    }
}
