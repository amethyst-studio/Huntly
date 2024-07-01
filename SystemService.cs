using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace Huntly;

#nullable disable
public class SystemService
{
    // Plugin Constants
    [PluginService] public static IDalamudPluginInterface PluginInterface { get; private set; }
    [PluginService] public static IFramework Framework { get; private set; }
    [PluginService] public static ICommandManager Commands { get; private set; }
    [PluginService] public static IClientState ClientState { get; private set; }
    [PluginService] public static ICondition Condition { get; private set; }
    [PluginService] public static ITargetManager Targets { get; private set; }
    [PluginService] public static IDataManager DataManager { get; private set; }

    [PluginService] public static IChatGui Chat { get; private set; }
    [PluginService] public static IObjectTable Objects { get; private set; }
    [PluginService] public static IAetheryteList AetheryteList { get; private set; }
    [PluginService] public static IPluginLog Log { get; private set; }

    // Global Constants
    public static WindowSystem WindowSystem { get; private set; }
    public static TerritoryManager TerritoryManager { get; private set; }

    // Logical Initialization
    internal static bool IsInitialized = false;
    public static void Initialize(IDalamudPluginInterface Interface)
    {
        try
        {
            if (IsInitialized)
            {
                Log.Debug("Internal Services Initialized.");
            }
            IsInitialized = true;

            Interface.Create<SystemService>();
            WindowSystem = new WindowSystem(Plugin.Name);
            TerritoryManager = new TerritoryManager();
        }
        catch (Exception except)
        {
            Console.WriteLine($"Failed to Load Service(s): {except.Message}");
        }
    }
}
