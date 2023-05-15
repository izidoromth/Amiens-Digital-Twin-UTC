using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public sealed class BuildingsDataInterface
    {
        const string buildingsTableName = "buildings";

        private static BuildingsDataInterface _instance;
        public static BuildingsDataInterface Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BuildingsDataInterface();
                }
                return _instance;
            }
        }

        public List<Building> Buildings { get; set; }

        private BuildingsDataInterface() { }

        public void RetrieveBuildingsData()
        {
            DataTable buildingsTable = PSQLInterface.PGSQLExecuteSelectQuery(buildingsTableName);

            Buildings = new List<Building>();

            foreach (DataRow building_row in buildingsTable.Rows)
            {
                Building newBuilding = new Building();
                newBuilding.Id = building_row.ItemArray[0] as string;
                newBuilding.Nature = building_row.ItemArray[1] as string;
                newBuilding.Usage1 = building_row.ItemArray[2] as string;
                newBuilding.Usage2 = building_row.ItemArray[3] as string;
                newBuilding.NbLogts = building_row.ItemArray[6];
                newBuilding.NbFloors = building_row.ItemArray[7];
                newBuilding.Height = building_row.ItemArray[8];
                newBuilding.ZMinSol = building_row.ItemArray[9];
                newBuilding.ZMinToit = building_row.ItemArray[10];
                newBuilding.Geometry = building_row.ItemArray[11] as byte[];

                Buildings.Add(newBuilding);
            }
        }

    }
}
