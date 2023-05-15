using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class FloodData
    {
        public int Id { get; set; }
        public int Time { get; set; }
        public string SectorId { get; set; }
        public float Height { get; set; }
        public int Year { get; set; }
    }
}
