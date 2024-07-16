namespace Unity.Services.CloudSave;

using Godot;
using Unity.Services.Authentication;
using Unity.Services.CloudSave.Internal;

public partial class CloudSaveService : Node
{
    public static CloudSaveService Instance { get; private set; }
    public DataService Data { get; private set; }
    public FilesService Files { get; private set; }

    public override void _EnterTree() => Instance = this;

    public override void _Ready()
    {
        AuthenticationService.Instance.SignedIn += OnSignedIn;
    }

    private void OnSignedIn()
    {
        Data = new DataService();
        Files = new FilesService();
    }

    public override void _ExitTree()
    {
        AuthenticationService.Instance.SignedIn -= OnSignedIn;
    }
}
