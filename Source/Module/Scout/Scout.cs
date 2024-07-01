
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;

namespace Huntly;

public class ScoutModule : UWindowModule
{
  private static MenuState ScoutMenuState = new MenuState();

  private static bool EnableSonar = false;

  public static ScoutModule Create()
  {
    return new ScoutModule();
  }

  public ScoutModule() : base("Scout", ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoScrollbar)
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
    // Menu Bar
    if (ImGui.BeginMenuBar())
    {
      // Tools Tab
      if (ImGui.BeginMenu("Tools"))
      {
        // Open Imports
        if (ImGui.MenuItem("Import", null, ScoutMenuState.GetState("Import.Open", false)))
        {
          bool ImportMenuOpen = ScoutMenuState.GetState("Import.Open", false);
          ScoutMenuState.SetState("Import.Open", !ImportMenuOpen);
        }

        // Open Exports
        if (ImGui.MenuItem("Export", null, ScoutMenuState.GetState("Export.Open", false)))
        {
          bool ExportMenuOpen = ScoutMenuState.GetState("Export.Open", false);
          ScoutMenuState.SetState("Export.Open", !ExportMenuOpen);
        }

        // Single Use - Reset
        if (ImGui.MenuItem("Reset", null, ScoutMenuState.GetState("Reset.DoAction", false)))
        {
          ScoutMenuState.SetState("Reset.DoAction", true);
        }
        ImGui.EndMenu();
      }
      ImGui.EndMenuBar();

      // Handle Reset
      if (ScoutMenuState.GetState("Reset.DoAction", false))
      {
        ScoutMenuState.SetState("Reset.DoAction", false);
        Marks.List.Clear();
      }

      // Enable Sonar Scanning
      ImGui.Checkbox("Enable Sonar", ref EnableSonar);
      if (EnableSonar) {
        LookForMark();
        ImGui.SameLine();
        ImGui.Text($"(Marks Registered = {Marks.List.Count()})");
      }

      // Render Scout Table
      if(ImGui.BeginTable("Mark List", 5, ImGuiTableFlags.Resizable | ImGuiTableFlags.Borders | ImGuiTableFlags.Hideable)) {
        ImGui.TableSetupColumn("Actions");
        ImGui.TableSetupColumn("Name");
        ImGui.TableSetupColumn("Location");
        ImGui.TableSetupColumn("X");
        ImGui.TableSetupColumn("Y");
        ImGui.TableHeadersRow();

        foreach (Mark Mark in Marks.List.ToList()) {
          ImGui.TableNextColumn();
          int Index = Marks.GetIndex(Mark);
          if(ImGui.ArrowButton($"UP##{Mark.GetName()}:{Mark.GetMap()}", ImGuiDir.Up))
          {
            Marks.List.Move(Index, MoveDirection.Up);
          }
          ImGui.SameLine();
          if(ImGui.ArrowButton($"DOWN##${Mark.GetName()}:{Mark.GetMap()}", ImGuiDir.Down))
          {
            Marks.List.Move(Index, MoveDirection.Down);
          }
          ImGui.SameLine();
          if (ImGui.Button($"X##{Mark.GetName()}:{Mark.GetMap()}"))
          {
            Marks.List.Remove(Mark);
          }

          ImGui.TableNextColumn();
          ImGui.PushItemWidth(ImGui.GetColumnWidth());
          ImGui.InputText($"##{Index}:{Mark.GetMap()}", ref Mark.IHuntName, 64);
          ImGui.PopItemWidth();

          ImGui.TableNextColumn();
          ImGui.Text($"{Mark.GetMap()}");

          ImGui.TableNextColumn();
          ImGui.Text($"{Mark.GetX()}");

          ImGui.TableNextColumn();
          ImGui.Text($"{Mark.GetY()}");

          ImGui.TableNextRow();
        }
        ImGui.EndTable();
      }

      // Render Import if Open
      if (ScoutMenuState.GetState("Import.Open", false))
      {
        if (ImGui.CollapsingHeader("Import Marks", ImGuiTreeNodeFlags.DefaultOpen))
        {
          ImGui.InputTextMultiline("##Import", ref ImportExport.TextInput, 65536, new Vector2(ImGui.GetWindowWidth() - 15, 200));
          if (ImGui.Button("Import Marks##Import.Do"))
          {
            ImportExport.DoImport();
          }
          ImGui.SameLine();
          if (ImGui.Button("Done##Import.Done")) {
            ScoutMenuState.SetState("Import.Open", false);
          }
          ImGui.SameLine();
          ImGui.Combo("##Import.Mode", ref ImportExport.ImportMode, ImportExport.ImportModes, ImportExport.ImportModes.Length);
        }
      }

      // Render Export if Open
      if (ScoutMenuState.GetState("Export.Open", false))
      {
        if (ImGui.CollapsingHeader("Export Marks", ImGuiTreeNodeFlags.DefaultOpen))
        {
          ImGui.InputTextMultiline("##Export", ref ImportExport.TextOutput, 65536, new Vector2(ImGui.GetWindowWidth() - 15, 200));
          if (ImGui.Button("Export Marks##Export.Do")) {
            ImportExport.DoExport();
          }
          ImGui.SameLine();
          if (ImGui.Button("Done##Export.Done")) {
            ScoutMenuState.SetState("Export.Open", false);
          }

          ImGui.SameLine();
          ImGui.Combo("##Export.Mode", ref ImportExport.ExportMode, ImportExport.ExportModes, ImportExport.ExportModes.Length);
        }
      }
    }
  }

  protected unsafe void LookForMark() {
    if (SystemService.ClientState.LocalPlayer == null) return;

    foreach (IGameObject Actor in SystemService.Objects) {
      if (Actor == null) continue;
      GameObject* ActorStruct = (GameObject*)Actor.Address;
      if (ActorStruct == null) continue;
      if (ActorStruct->EventId.ContentId == EventHandlerType.MobHunt) {
        TerritoryDetail TerrDetail = SystemService.TerritoryManager.GetByTerritoryType(SystemService.ClientState.TerritoryType)!;
        Vector3 MapCoordinates = Actor.GetMapCoordinates();
        Mark Mark = new Mark($"{TerrDetail.Name} : {Actor.Name} : {MapCoordinates.X:0.0} : {MapCoordinates.Y:0.0}");
        if(!Marks.Registered(Mark)) {
          SystemService.Log.Information($"Hunt Found: {Actor.Name} {TerrDetail.MapId}/{TerrDetail.Name} {MapCoordinates}");
          Marks.Upsert(Mark);
        }
      }
    }
  }

  protected override void EDispose()
  {
  }
}
