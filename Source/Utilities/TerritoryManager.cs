
using Dalamud.Plugin.Services;
using Lumina.Excel.GeneratedSheets;

namespace Huntly;

public class TerritoryManager
{
  private readonly IDataManager DataManager;
  private readonly IEnumerable<TerritoryDetail> TerritoryDetails;

  public TerritoryManager()
  {
    DataManager = SystemService.DataManager;
    TerritoryDetails = LoadTerritoryDetails();
  }

  public TerritoryDetail? GetByZoneName(string Zone, bool Partial = true)
  {
    if (!TerritoryDetails.Any()) LoadTerritoryDetails();

    var TerrDetails =
        TerritoryDetails
            .Where(x => x.Name.Equals(Zone, StringComparison.OrdinalIgnoreCase) ||
                        Partial && x.Name.ToLower().Contains(Zone.ToLower())).OrderBy(x => x.Name.Length);

    var TerrDetail = TerrDetails.FirstOrDefault();

    return TerrDetail!;
  }

  private IEnumerable<TerritoryDetail> LoadTerritoryDetails()
  {
    return (from TerrType in DataManager.GetExcelSheet<TerritoryType>()
            let Type = TerrType.Bg.RawString.Split('/')
            where Type.Length >= 3
            where Type[2] == "twn" || Type[2] == "fld" || Type[2] == "hou"
            where !string.IsNullOrWhiteSpace(TerrType.Map.Value.PlaceName.Value.Name)
            select new TerritoryDetail
            {
              TerritoryType = TerrType.RowId,
              MapId = TerrType.Map.Value.RowId,
              SizeFactor = TerrType.Map.Value.SizeFactor,
              Name = TerrType.Map.Value.PlaceName.Value.Name
            }).ToList();
  }

  public TerritoryDetail? GetByTerritoryType(ushort territoryType)
  {
    return TerritoryDetails.FirstOrDefault(x => x.TerritoryType == territoryType);
  }
}

public class TerritoryDetail
{
  public string Name { get; set; } = null!;
  public uint TerritoryType { get; set; }
  public uint MapId { get; set; }
  public ushort SizeFactor { get; set; }
}
