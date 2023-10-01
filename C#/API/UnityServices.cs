// Reference: https://services.docs.unity.com/docs/service-account-auth/

using System;
using Godot;

namespace Unity.Services.Core;

public partial class UnityServices : HttpRequest
{
    public static UnityServices Instance { get; private set; }
    private const string APIEndpoint = "https://services.api.unity.com";

    [Export(PropertyHint.ResourceType)]
    private APIResource apiResource;
    public string ProjectId => apiResource.ProjectId;
    public event Action<bool> OnInitialize;

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        RequestCompleted += HttpRequestCompleted;
    }

    public Error Initialize() =>
        Request(
            APIEndpoint,
            new string[] { $"Authorization: Basic {apiResource.ServiceAccountCredentials}" }
        );

    private void HttpRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
    {
        Error error = (Error)result;
        GD.Print(error.ToString());
        OnInitialize?.Invoke(error == Error.Ok);
    }

    public override void _ExitTree()
    {
        RequestCompleted -= HttpRequestCompleted;
    }
}
