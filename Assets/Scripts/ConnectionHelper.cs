using DatabaseConnection.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    static class ConnectionHelper
    {
        public static AmiensDigitalTwinDbContext Context { get; set; }
    }
}
