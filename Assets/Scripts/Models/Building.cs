using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models
{
    public class Building : CityObject
    {
        public string Nature { get; set; }
        public string Usage1 { get; set; }
        public string Usage2 { get; set; }
        public object NbLogts { get; set; }
        public object NbFloors { get; set; }
        public object Height { get; set; }
        public object ZMinSol { get; set; }
        public object ZMinToit { get; set; }
    }
}
