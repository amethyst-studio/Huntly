
namespace Huntly;

public class MenuState
{
  private Dictionary<string, bool> kv = new Dictionary<string, bool>();

  public bool GetState(string id, bool v)
  {
    return kv.GetValueOrDefault(id, v);
  }

  public void SetState(string id, bool v)
  {
    if (kv.ContainsKey(id)) kv.Remove(id);
    kv.Add(id, v);
  }
}
