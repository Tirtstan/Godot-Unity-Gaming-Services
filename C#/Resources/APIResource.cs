using Godot;

namespace Unity.Services.Core;

[GlobalClass]
public partial class APIResource : Resource
{
    [Export(PropertyHint.Password)]
    public string ServiceAccountCredentials { get; set; }

    [Export(PropertyHint.Password)]
    public string ProjectId { get; set; }
}
