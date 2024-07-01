
using System.Numerics;

namespace Huntly;

public static class Marks
{
  public static List<Mark> List = new List<Mark>();

  public static void Upsert(Mark Mark) {
    int Index = List.FindIndex(M => M.GetName() == Mark.GetName());
    if (Index == -1) List.Add(Mark);
    else List[Index] = Mark;
  }

  public static int GetIndex(Mark Mark) {
    return List.FindIndex(M => M.GetName() == Mark.GetName());
  }

  public static bool Registered(Mark Mark) {
    return List.Find(M => M.GetName() == Mark.GetName()) != null;
  }
}

public class Mark
{
  private string[] HuntString;
  public string IHuntName;

  public Mark(string HuntString)
  {
    this.HuntString = HuntString.Split(" : ");
    IHuntName = this.HuntString[1].Trim();
  }

  public string GetName()
  {
    return IHuntName.Trim();
  }

  public string GetMap()
  {
    return HuntString[0].Trim();
  }

  public string GetX()
  {
    return HuntString[2].Trim();
  }

  public string GetY()
  {
    return HuntString[3].Trim();
  }
}
