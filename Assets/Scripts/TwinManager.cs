using Assets.Scripts;
using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static TreeEditor.TextureAtlas;

public class TwinManager : MonoBehaviour
{
    List<GameObject> floodSectorGameObjects = new List<GameObject>();
    float time;
    List<FloodData> aux = new List<FloodData>();


    void Start()
    {
        // Connect to the database
        PSQLInterface.PSQLConnect("127.0.0.1", "5433", "amiens_digital_twin", "postgres", "12345678");

        // Retrieve city objects
        // terrain
        // buildings
        // instantiate city objects

        FloodDataInterface.Instance.RetrieveFloodSectorsData();
        FloodDataInterface.Instance.RetrieveFloodData();

        InstantiateFloodSectors();

        // add object components (behaviors)
        InvokeRepeating("UpdateWaterLevel", 0, 0.0008f);

        // wait for user inputs
        PSQLInterface.PGSQLCloseConnection();

        time = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void InstantiateFloodSectors()
    {
        foreach(FloodSector sector in FloodDataInterface.Instance.FloodSectors)
        {
            MemoryStream stream = new MemoryStream(sector.Geometry);
            GameObject sectorGameObject = new OBJLoader().Load(stream);
            sectorGameObject.name = sector.Id;

            // apply texture to the flood sections
            Texture2D tex = new Texture2D(2, 2);
            TextAsset imageAsset = Resources.Load<TextAsset>("Texture.jpg");
            tex.LoadImage(imageAsset.bytes);
            sectorGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.mainTexture = tex;

            floodSectorGameObjects.Add(sectorGameObject);
        }
    }

    void UpdateWaterLevel()
    {
        //Debug.Log(Time.realtimeSinceStartup - time);
        foreach(GameObject floodSector in floodSectorGameObjects)
        {
            var floodSectorData = FloodDataInterface.Instance.FloodsPerYear[9999].FirstOrDefault(f => f.SectorId.Equals(floodSector.name));
            if(floodSectorData == null)
            {
                FloodDataInterface.Instance.FloodsPerYear[9999] = aux;
                return;
            }
            Debug.Log(floodSectorData.Time);
            floodSector.transform.position = new Vector3(floodSector.transform.position.x, floodSectorData.Height, floodSector.transform.position.z);
            aux.Add(floodSectorData);
            FloodDataInterface.Instance.FloodsPerYear[9999].Remove(floodSectorData);
        }
        time = Time.realtimeSinceStartup;
    }
}
