namespace Unity.Services.CloudSave;

using Godot;
using Unity.Services.Authentication;
using Unity.Services.CloudSave.Internal;
using Unity.Services.Core;

public partial class CloudSaveService : Node
{
    public static CloudSaveService Instance { get; private set; }
    public DataService Data { get; private set; }
    public FilesService Files { get; private set; }

    private static string ProjectId => UnityServices.Instance.ProjectId;
    private static string PlayerId => AuthenticationService.Instance.PlayerId;

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        AuthenticationService.Instance.SignedIn += OnSignedIn;
    }

    private void OnSignedIn()
    {
        Data = new DataService(ProjectId, PlayerId);
        Files = new FilesService(ProjectId, PlayerId);
    }

    public override void _ExitTree()
    {
        AuthenticationService.Instance.SignedIn -= OnSignedIn;
    }
}
