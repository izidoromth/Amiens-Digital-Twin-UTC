using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Scripts
{
    public class FloodSector
    {
        public string Id { get; set; }
        public byte[] Geometry { get; set; }
    }

    public class FloodData
    {
        public int Id { get; set; }
        public int Time { get; set; }
        public string SectorId { get; set; }
        public float Height { get; set; }
        public int Year { get; set; }

    }

    public sealed class FloodDataInterface
    {
        const string FloodSectorsTableName = "flood_sectors";
        const string WaterFloodTableName = "water_flood";

        public List<FloodSector> FloodSectors { get; set; }
        public List<FloodData> FloodData { get; set; }
        public Dictionary<int, List<FloodData>> FloodsPerYear { get; set; }

        private static FloodDataInterface _instance;
        public static FloodDataInterface Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FloodDataInterface();
                }
                return _instance;
            }
        }

        private FloodDataInterface() { }

        public void RetrieveFloodSectorsData()
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

        public void RetrieveFloodData()
        {
            DataTable floodsTable = PSQLInterface.PGSQLExecuteSelectQuery(WaterFloodTableName);

            FloodData = new List<FloodData>();
            FloodsPerYear = new Dictionary<int, List<FloodData>>();

            foreach (DataRow flood in floodsTable.Rows)
            {
                FloodData newFloodData = new FloodData();
                newFloodData.Id = (int)flood.ItemArray[0];
                newFloodData.Time = (int)flood.ItemArray[1];
                newFloodData.SectorId = (string)flood.ItemArray[2];
                newFloodData.Height = (float)flood.ItemArray[3];
                newFloodData.Year = (int)flood.ItemArray[4];

                FloodData.Add(newFloodData);
            }

            foreach (var year in FloodData.GroupBy(x => x.Year))
            {
                FloodsPerYear.Add(year.Key, year.OrderBy(x => x.Time).ToList());
            }
        }
    }
}
