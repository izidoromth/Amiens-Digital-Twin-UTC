using DatabaseConnection.Entities;
using System.Collections.Generic;

namespace APIRequest
{
    public interface IRepository
    {
        public IList<Building> GetBuildings();
        public Building? GetBuildingById(string id);
        public IList<Terrain> GetTerrains();
        public IList<WaterFlood> GetWaterFloodsByYear(int year);
        public IList<WaterFlood> GetWaterFloodsByYearAndRange(int year, int? initialTime, int? finalTime);
        public IList<WaterFlood> GetWaterFloodsByYearAndSectorId(int year, string sectorId);
        public IList<FloodSector> GetFloodSectors();
    }
}
