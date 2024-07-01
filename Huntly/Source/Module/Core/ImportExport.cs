
using System.Text.RegularExpressions;

namespace Huntly;

public static class ImportExport
{
  public static string[] ImportModes = ["Huntly", "Wobbuffet", "Request More!"];
  public static int ImportMode = 0;
  public static string TextInput = "";

  public static string[] ExportModes = ["Huntly", "Request More!"];
  public static int ExportMode = 0;
  public static string TextOutput = "";

  public static void DoImport()
  {
    SystemService.Log.Information("Importing Marks...");

    switch (ImportMode)
    {
      case 0:
        {
          foreach (string Mark in TextInput.Split("\n"))
          {
            if (Mark.Trim() == "") continue;
            Mark Parse = new Mark(Mark);
            try {
              SystemService.Log.Information($"Import - {Parse.GetName()} {Parse.GetMap()} X:{Parse.GetX()} Y:{Parse.GetY()}");
              Marks.Upsert(Parse);
            } catch {
              SystemService.Log.Error($"Failed to Import '{Mark}' with {ImportModes[ImportMode]}.");
            }
          }
          break;
        }
      case 1:
        {
          string Location = "";
          string Instance = "";
          foreach (string Mark in TextInput.Split("\n"))
          {
            if (Mark.Trim() == "") continue;
            string[] Data = Mark.Trim().Replace(")", "").Replace(",", "").Split(" (");
            if (Data.Count() == 1) {
              Instance = "";
              Match MatchedNumber = Regex.Match(Data[0], "[0-9]{1,}");
              Location = Regex.Replace(Data[0], "[0-9]{1,}", "");
              Instance = MatchedNumber.ToString();
              if (Instance.Trim() != "") {
                Instance = $"(Instance {Instance})";
              }
              continue;
            }
            string X = Data[1].Split(" ")[0];
            string Y = Data[1].Split(" ")[1];
            Mark Parse = new Mark($"{Location} : {Data[0]} {Instance} : {X} : {Y}");
            try {
              SystemService.Log.Information($"Import - {Parse.GetName()} {Parse.GetMap()} X:{Parse.GetX()} Y:{Parse.GetY()}");
              Marks.Upsert(Parse);
            } catch {
              SystemService.Log.Error($"Failed to Import '{Mark}' with {ImportModes[ImportMode]}.");
            }
          }
          break;
        }
    }
  }

  public static void DoExport()
  {
    SystemService.Log.Information("Exporting Marks...");
    TextOutput = "";

    switch (ExportMode)
    {
      case 0:
      {
        foreach (Mark Mark in Marks.List)
        {
          TextOutput += $"{Mark.GetMap()} : {Mark.GetName()} : {Mark.GetX()} : {Mark.GetY()}\n";
        }
        break;
      }
    }
  }
}
