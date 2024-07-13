#if TOOLS
using Godot;

[Tool]
public partial class GodotUGS : EditorPlugin
{
    private const string AutoloadName = "GodotUGS";

    public override void _EnterTree()
    {
        AddAutoloadSingleton(AutoloadName, "res://addons/GodotUGS/Autoloads/GodotUGS.tscn");
    }

    public override void _ExitTree()
    {
        RemoveAutoloadSingleton(AutoloadName);
    }
}
#endif
