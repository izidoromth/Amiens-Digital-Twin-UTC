using Assets.Scripts;
using Assets.Scripts.DataInterfaces;
using Assets.Scripts.Models;
using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static TreeEditor.TextureAtlas;

public class TwinManager : MonoBehaviour
{
    List<GameObject> floodSectorGameObjects = new List<GameObject>();
    List<FloodData> aux = new List<FloodData>();


    void Start()
    {
        // Connect to the database
        PSQLInterface.PSQLConnect("127.0.0.1", "5433", "amiens_digital_twin", "postgres", "12345678");

        // Retrieve city objects
        FloodDataInterface.Instance.RetrieveFloodSectorsData();
        FloodDataInterface.Instance.RetrieveFloodData();
        BuildingsDataInterface.Instance.RetrieveBuildingsData();
        TerrainDataInterface.Instance.RetrieveTerrainData();

        // Instantiate city objects
        InstantiateFloodSectors();
        InstantiateBuildings();
        InstantiateTerrains();

        // Flood tests
        InvokeRepeating("UpdateWaterLevel", 0, 0.0008f);

        PSQLInterface.PGSQLCloseConnection();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void InstantiateFloodSectors()
    {
        foreach(FloodSector sector in FloodDataInterface.Instance.FloodSectors)
        {
            GameObject sectorGameObject = LoadFromGeometry(sector, "Texture.jpg");

            floodSectorGameObjects.Add(sectorGameObject);
        }
    }

    void InstantiateBuildings()
    {
        foreach (Building building in BuildingsDataInterface.Instance.Buildings)
        {
            LoadFromGeometry(building, "Buildings.jpg");
        }
    }

    void InstantiateTerrains()
    {
        foreach (Assets.Scripts.Models.Terrain terrain in TerrainDataInterface.Instance.Terrains)
        {
            LoadFromGeometry(terrain);
        }
    }
    GameObject LoadFromGeometry(CityObject city_object, string texture_name = "")
    {
        MemoryStream stream = new MemoryStream(city_object.Geometry);
        GameObject gameObject = new OBJLoader().Load(stream);
        gameObject.name = city_object.Id as string;

        if (string.IsNullOrEmpty(texture_name))
            return gameObject;

        // apply texture to the gameobject
        Texture2D tex = new Texture2D(2, 2);
        TextAsset imageAsset = Resources.Load<TextAsset>(texture_name);
        tex.LoadImage(imageAsset.bytes);
        gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.mainTexture = tex;

        return gameObject;
    }

    void UpdateWaterLevel()
    {
        foreach(GameObject floodSector in floodSectorGameObjects)
        {
            var floodSectorData = FloodDataInterface.Instance.FloodsPerYear[9999].FirstOrDefault(f => f.SectorId.Equals(floodSector.name));
            if(floodSectorData == null)
            {
                FloodDataInterface.Instance.FloodsPerYear[9999] = aux;
                return;
            }
            floodSector.transform.position = new Vector3(floodSector.transform.position.x, floodSectorData.Height, floodSector.transform.position.z);
            aux.Add(floodSectorData);
            FloodDataInterface.Instance.FloodsPerYear[9999].Remove(floodSectorData);
        }
    }
}
