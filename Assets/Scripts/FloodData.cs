using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class FloodSector
    {
        public string Id { get; set; }
        public byte[] Geometry { get; set; }
    }

    public sealed class FloodData
    {
        const string FloodSectorsTableName = "flood_sectors";

        public List<FloodSector> FloodSectors { get; set; }

        private static FloodData _instance;
        public static FloodData Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FloodData();
                }
                return _instance;
            }
        }

        private FloodData() { }

        public void RetrieveFloodData()
        {
            DataTable floodSectorsTable = PSQLInterface.PGSQLExecuteSelectQuery(FloodSectorsTableName);

            FloodSectors = new List<FloodSector>();

            foreach (DataRow flood_sector_row in floodSectorsTable.Rows)
            {
                FloodSector newFloodSector = new FloodSector();
                newFloodSector.Id = flood_sector_row.ItemArray[0] as string;
                newFloodSector.Geometry = flood_sector_row.ItemArray[1] as byte[];

                FloodSectors.Add(newFloodSector);
            }
        }
    }
}
