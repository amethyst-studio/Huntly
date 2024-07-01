
using ImGuiNET;

namespace Huntly;

public class ConductorModule : UWindowModule
{
  public static ConductorModule Create()
  {
    return new ConductorModule();
  }

  public ConductorModule() : base("Conductor", ImGuiWindowFlags.NoScrollbar)
  {
  }

  protected override void LoadModule()
  {

  }

  protected override void SetWindowOptions()
  {
  }

  public override void Draw()
  {
    ImGui.Text("HELLO WORLD");
  }

  protected override void EDispose()
  {
  }
}
