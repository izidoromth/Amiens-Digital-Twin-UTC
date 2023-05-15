using Assets.Scripts.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DataInterfaces
{
    public class TerrainDataInterface
    {
        const string terrainsTableName = "terrains";

        private static TerrainDataInterface _instance;
        public static TerrainDataInterface Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TerrainDataInterface();
                }
                return _instance;
            }
        }

        public List<Terrain> Terrains { get; set; }

        private TerrainDataInterface() { }

        public void RetrieveTerrainData()
        {
            DataTable terrainsTable = PSQLInterface.PGSQLExecuteSelectQuery(terrainsTableName);

            Terrains = new List<Terrain>();

            foreach (DataRow terrain_row in terrainsTable.Rows)
            {
                Terrain newTerrain = new Terrain();
                newTerrain.Id = (int)terrain_row.ItemArray[0];
                newTerrain.Geometry = terrain_row.ItemArray[1] as byte[];
                newTerrain.Name = (string)terrain_row.ItemArray[2];

                Terrains.Add(newTerrain);
            }
        }
    }
}
