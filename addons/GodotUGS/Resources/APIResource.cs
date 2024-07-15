using Godot;

namespace Unity.Services.Core;

#if GODOT4_1_OR_GREATER
[GlobalClass]
#endif
public partial class APIResource : Resource
{
    [Export(PropertyHint.Password)]
    public string ProjectId { get; set; }
}
