namespace Unity.Services.Core;

// References: https://services.docs.unity.com/docs/service-account-auth/ && https://restsharp.dev/docs/usage/basics
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using RestSharp;

public partial class UnityServices : Node
{
    public static UnityServices Instance { get; private set; }
    public string Environment => initializationOptions.Environment;
    public string ProjectId => apiResource.ProjectId;
    public event Action<bool> OnInitialize;

    public Dictionary<string, string> DefaultHeaders =>
        new() { { "ProjectId", ProjectId }, { "UnityEnvironment", Environment } };

    private RestClient restClient;
    private const string UnityServicesURL = "https://services.api.unity.com";
    private InitializationOptions initializationOptions = new();

    [Export(PropertyHint.ResourceType)]
    private APIResource apiResource;

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        restClient = new RestClient(UnityServicesURL);
    }

    public async Task InitializeAsync(InitializationOptions options = null)
    {
        if (apiResource == null)
            throw new NullReferenceException("API Resource is not set!");

        if (string.IsNullOrEmpty(apiResource.ProjectId))
            throw new NullReferenceException("Project ID is not set!");

        if (options != null)
            initializationOptions = options;

        var request = new RestRequest();
        var response = await restClient.ExecuteAsync(request);
        OnInitialize?.Invoke(response.IsSuccessful);

        if (!response.IsSuccessful)
        {
            if (response.ErrorException == null)
                throw new ServicesInitializationException("Failed to initialize Unity Services");

            throw response.ErrorException;
        }
    }
}
