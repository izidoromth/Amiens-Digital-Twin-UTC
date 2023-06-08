using APIRequest;
using DatabaseConnection.Context;
using DatabaseConnection.Entities;
using System.Collections.Generic;
using System.Linq;

public class LocalRepository : IRepository
{
    private readonly AmiensDigitalTwinDbContext _context;
    public LocalRepository()
    {
        _context = DbConnectionContext.GetContext<AmiensDigitalTwinDbContext>(
            (options, defaultSchema) => { return new AmiensDigitalTwinDbContext(options, defaultSchema); },
            "postgres", "postgres", "amiens_digital_twin");
    }
    public Building GetBuildingById(string id)
    {
        return _context.Buildings.FirstOrDefault(x => x.Id == id);
    }

    public IList<Building> GetBuildings()
    {
        return _context.Buildings.ToList();
    }

    public IList<FloodSector> GetFloodSectors()
    {
        return _context.FloodSectors.ToList();
    }

    public IList<Terrain> GetTerrains()
    {
        return _context.Terrains.ToList();
    }

    public IList<WaterFlood> GetWaterFloodsByYear(int year)
    {
        return _context.WaterFloods.Where(x => x.Year == year).OrderBy(x => x.SectorId).ToList();
    }

    public IList<WaterFlood> GetWaterFloodsByYearAndRange(int year, int? initialTime, int? finalTime)
    {
        int iTime = initialTime.HasValue ? initialTime.Value : 0;
        int fTime = finalTime.HasValue ? finalTime.Value : int.MaxValue;
        return _context.WaterFloods
            .Where(x => x.Year.Value == year && x.Time.Value >= iTime && x.Time.Value <= fTime)
            .ToList();
    }

    public IList<WaterFlood> GetWaterFloodsByYearAndSectorId(int year, string sectorId)
    {
        return _context.WaterFloods
                .Where(x => x.Year == year && x.SectorId == sectorId)
                .ToList();
    }
}
