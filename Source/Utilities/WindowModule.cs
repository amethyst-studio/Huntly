
using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace Huntly;

public abstract class UWindowModule : Window, IDisposable
{
  protected string name;

  public UWindowModule(string name, ImGuiWindowFlags flags) : base(name, flags)
  {
    this.name = name;
    Plugin.Disposable.Add(this);

    // Create Window
    SetWindowOptions();
    SystemService.WindowSystem.AddWindow(this);
    AddMainWindowCommand();

    // Create Module
    LoadModule();
  }

  private void AddMainWindowCommand()
  {
    SystemService.Commands.AddHandler($"/{name.ToLower()}", new CommandInfo(
        (command, args) =>
        {
          ToggleWindow();
        }
    )
    {
      HelpMessage = $"Open the '{name}' Module Main Window."
    });
  }

  protected void ToggleWindow()
  {
    Toggle();
  }

  protected abstract void LoadModule();

  protected abstract void SetWindowOptions();

  protected abstract void EDispose();

  public void Dispose()
  {
    EDispose();
    SystemService.Commands.RemoveHandler($"/{name.ToLower()}");
  }
}
