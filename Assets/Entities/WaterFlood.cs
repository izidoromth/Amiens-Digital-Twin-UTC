using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseConnection.Entities
{
    public class WaterFlood
    {
        public int Id { get; set; }
        public int? Time { get; set; }
        public string? SectorId { get; set; }
        public float? Level { get; set; }
        public int? Year { get; set; }
    }
}
