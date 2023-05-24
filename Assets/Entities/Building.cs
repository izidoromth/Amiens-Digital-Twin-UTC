using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseConnection.Entities
{
    public class Building
    {
        public string Id { get; set; }
        public string? Nature { get; set; }
        public string? Usage1 { get; set; }
        public string? Usage2 { get; set; }
        public string? NbLogts { get; set; }
        public int? NbEtages { get; set; }
        public float? Hauter { get; set; }
        public float? ZMinSol { get; set; }
        public float? ZMinToit { get; set; }
        public byte[]? Geometry { get; set; }
    }
}
