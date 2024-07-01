using Dalamud.Plugin;

namespace Huntly;

public sealed class Plugin : IDalamudPlugin
{
  public static string Name = "Huntly";
  public static HashSet<IDisposable> Disposable = new HashSet<IDisposable>();

  public Plugin(
      IDalamudPluginInterface Interface
  )
  {
    // Build Service Interface
    SystemService.Initialize(Interface);

    // Register WindowSystem
    SystemService.PluginInterface.UiBuilder.Draw += SystemService.WindowSystem.Draw;

    // Register Module
    var x = ScoutModule.Create();
    // ConductorModule.Create();
    x.Toggle();
  }
  public void Dispose()
  {
    // Automatically Purge Disposables
    foreach (var Disposing in Disposable)
    {
      Disposing.Dispose();
    }

    // Remove Windows
    SystemService.PluginInterface.UiBuilder.Draw -= SystemService.WindowSystem.Draw;
    SystemService.WindowSystem.RemoveAllWindows();
  }
}
