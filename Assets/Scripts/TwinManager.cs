using Assets.Scripts;
using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TwinManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Connect to the database
        PSQLInterface.PSQLConnect("127.0.0.1", "5433", "amiens_digital_twin", "postgres", "12345678");

        // Retrieve city objects
            // terrain
            // buildings
        // instantiate city objects

        FloodData.Instance.RetrieveFloodData();
        InstantiateFloodSectors();

        // add object components (behaviors)

        // wait for user inputs
        PSQLInterface.PGSQLCloseConnection();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void InstantiateFloodSectors()
    {
        foreach(FloodSector sector in FloodData.Instance.FloodSectors)
        {
            MemoryStream stream = new MemoryStream(sector.Geometry);
            GameObject sectorGameObject = new OBJLoader().Load(stream);
        }
    }
}
