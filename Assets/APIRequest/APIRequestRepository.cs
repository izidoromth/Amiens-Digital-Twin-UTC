using DatabaseConnection.Entities;
using System.Collections.Generic;

namespace APIRequest
{
    public class APIRequestRepository : IRepository
    {
        private const string apiUrl = "https://localhost:7235";

        private readonly string buildingEndpoint = $"{apiUrl}/Building";
        private readonly string floodSectorEndpoint = $"{apiUrl}/FloodSector";
        private readonly string terrainEndpoint = $"{apiUrl}/Terrain";
        private readonly string waterFloodEndpoint = $"{apiUrl}/WaterFlood";

        private readonly HttpClientService httpClientService = new HttpClientService();
        public Building? GetBuildingById(string id)
        {
            return httpClientService.Get<Building?>($"{buildingEndpoint}/{id}");
        }

        public IList<Building> GetBuildings()
        {
            return httpClientService.Get<IList<Building>>($"{buildingEndpoint}");
        }

        public IList<FloodSector> GetFloodSectors()
        {
            return httpClientService.Get<IList<FloodSector>>($"{floodSectorEndpoint}");
        }

        public IList<Terrain> GetTerrains()
        {
            return httpClientService.Get<IList<Terrain>>($"{terrainEndpoint}");
        }

        public IList<WaterFlood> GetWaterFloodsByYear(int year)
        {
            return httpClientService.Get<IList<WaterFlood>>($"{waterFloodEndpoint}/{year}");
        }

        public IList<WaterFlood> GetWaterFloodsByYearAndRange(int year, int? initialTime, int? finalTime)
        {
            return httpClientService
                .Get<IList<WaterFlood>>($"{waterFloodEndpoint}/{year}/range?initialTime={initialTime}&finalTime={finalTime}");
        }

        public IList<WaterFlood> GetWaterFloodsByYearAndSectorId(int year, string sectorId)
        {
            return httpClientService
                .Get<IList<WaterFlood>>($"{waterFloodEndpoint}/{year}/{sectorId}");
        }
    }
}
