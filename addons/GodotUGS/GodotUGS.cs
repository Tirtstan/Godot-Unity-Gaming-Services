#if TOOLS
namespace GodotUGS;

using Godot;

[Tool]
public partial class GodotUGS : EditorPlugin
{
    private const string AutoloadName = "GodotUGS";

    public override void _EnterTree()
    {
        AddAutoloadSingleton(AutoloadName, "res://addons/GodotUGS/Autoloads/GodotUGS.tscn");
    }

    public override void _EnablePlugin()
    {
        GD.PrintRich(
            "[b]If not, please install RestSharp. Open the terminal and run the following command:\n"
                + "[code]dotnet add package RestSharp[/code]"
        );
        GD.PrintRich(
            "[b]Please set the API resource in 'res://addons/GodotUGS/Autoloads/GodotUGS.tscn'\n"
                + "An example has been provided in 'res://addons/GodotUGS/Resources/APIResource_Example.tres'"
        );
    }

    public override void _ExitTree()
    {
        RemoveAutoloadSingleton(AutoloadName);
    }
}
#endif
