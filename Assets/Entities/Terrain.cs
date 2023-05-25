using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseConnection.Entities
{
    public class Terrain
    {
        public int Id { get; set; }
        public byte[]? Geometry { get; set; }
        public string? Name { get; set; }
    }
}