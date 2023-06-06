using Assets.Scripts;
using Dummiesman;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using DatabaseConnection;
using DatabaseConnection.Entities;
using DatabaseConnection.Context;
using static TreeEditor.TextureAtlas;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using XCharts.Runtime;

public class TwinManager : MonoBehaviour
{
    List<GameObject> floodSectorGameObjects = new List<GameObject>();
    List<WaterFlood> aux = new List<WaterFlood>();
    Dictionary<int, List<WaterFlood>> floodsPerYear = new Dictionary<int, List<WaterFlood>>();
    bool pumping;

    Dictionary<string, float> fondCasier = new Dictionary<string, float>()
    {
        { "KE02", 23.6f },
        { "KE03", 22.1f },
        { "KE06", 23.7f },
        { "KE07", 22.8f },
        { "KE08", 21.9f },
        { "KE10", 24.6f },
        { "KE11", 21.9f },
        { "KE12", 21.9f },
        { "KE14", 22.3f },
        { "KF00", 25.7f },
        { "KF01", 24.9f },
        { "KF02", 24.9f },
        { "KF03", 23.3f }
    };

    Dictionary<string, float> areaCasier = new Dictionary<string, float>()
    {
        { "KE02", 150830.717f },
        { "KE03", 215175.711f },
        { "KE06", 143133.145f },
        { "KE07", 120782.798f },
        { "KE08", 246189.225f },
        { "KE10", 84430.545f },
        { "KE11", 78968.871f },
        { "KE12", 131205.816f },
        { "KE14", 55542.03f },
        { "KF00", 72302.153f },
        { "KF01", 10845.615f },
        { "KF02", 32622.003f },
        { "KF03", 68127.322f }
    };

    public AmiensDigitalTwinDbContext Context;

    public List<WaterFlood> SelectedFlood;

    public Component ActiveBuildingInfo;
    public bool Playing { get; set; }
    public float FloodMaxThreshold { get; set; }
    public float FloodMinThreshold { get; set; }
    public int SelectedFloodYear { get; set; }
    public string SelectedPumpCasier { get; set; }

    void Start()
    {
        // Connect to the database
        Context = DbConnectionContext.GetContext<AmiensDigitalTwinDbContext>(
            (options, defaultSchema) => { return new AmiensDigitalTwinDbContext(options, defaultSchema); },
            "postgres", "12345678", "amiens_digital_twin", port: "5433");

        floodsPerYear.Add(1994, Context.WaterFloods.Where(f => f.Year == 1994).OrderBy(f => f.Time).ToList());
        floodsPerYear.Add(9999, Context.WaterFloods.Where(f => f.Year == 1994).OrderBy(f => f.Time).ToList());

        // Instantiate city objects
        InstantiateFloodSectors();
        InstantiateBuildings();
        InstantiateTerrains();
    }

    void Update()
    {
        HandleInputs();
    }

    void InstantiateFloodSectors()
    {
        foreach(FloodSector sector in Context.FloodSectors.ToList())
        {
            GameObject floodSectorGameObject = LoadFromGeometry(sector.Geometry, sector.SectorId);

            floodSectorGameObjects.Add(floodSectorGameObject);
            floodSectorGameObject.SetActive(false);

            // add mesh collider to detect flood
            GameObject floodSector = floodSectorGameObject.transform.GetChild(0).gameObject;
            floodSector.AddComponent<MeshCollider>();

            // change material to make it transparent
            Material objectMaterial = new Material(Shader.Find("Standard"));
            objectMaterial.SetColor("_Color", new Color(.08f, .26f, .57f, .4f));
            objectMaterial.SetFloat("_Mode", 3);
            objectMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            objectMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            objectMaterial.EnableKeyword("_ALPHABLEND_ON");
            objectMaterial.renderQueue = 3000;
            floodSectorGameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = objectMaterial;
        }
    }

    void InstantiateBuildings()
    {
        foreach (Building building in Context.Buildings.ToList())
        {
            GameObject buildingGameObject =  LoadFromGeometry(building.Geometry, building.Id, "Buildings.jpg");

            Outline outline = buildingGameObject.transform.GetChild(0).gameObject.AddComponent<Outline>();
            outline.enabled = false;
            outline.OutlineWidth = 8;

            buildingGameObject.SetActive(false);
            HandleBuildingInfo buildingInfoComponent =  buildingGameObject.transform.GetChild(0).gameObject.AddComponent<HandleBuildingInfo>();
            buildingInfoComponent.BuildingInfo = building;
            MeshCollider buildingMeshCollider = buildingGameObject.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
            Rigidbody buildingRigidbody = buildingGameObject.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
            buildingRigidbody.useGravity = false;
            buildingRigidbody.isKinematic = true;
            buildingGameObject.SetActive(true);

            buildingMeshCollider.convex = true;
            buildingMeshCollider.isTrigger = true;
        }
    }

    void InstantiateTerrains()
    {
        foreach (DatabaseConnection.Entities.Terrain terrain in Context.Terrains.ToList())
        {
            LoadFromGeometry(terrain.Geometry, terrain.Id.ToString(),"Terrain.png");
        }
    }
    
    GameObject LoadFromGeometry(byte[] geometry, string id, string texture_name = "")
    {
        MemoryStream stream = new MemoryStream(geometry);
        GameObject gameObject = new OBJLoader().Load(stream);
        gameObject.name = id;

        if (string.IsNullOrEmpty(texture_name))
            return gameObject;

        // apply texture to the gameobject
        Texture2D tex = new Texture2D(2, 2);
        TextAsset imageAsset = Resources.Load<TextAsset>(texture_name);
        tex.LoadImage(imageAsset.bytes);
        gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.mainTexture = tex;

        return gameObject;
    }

    public void PlaySimulation(int speed)
    {
        Playing = true;
        foreach (GameObject floodSector in floodSectorGameObjects) { floodSector.SetActive(true); }
        SelectedFlood = floodsPerYear[SelectedFloodYear].ToList();
        aux = new List<WaterFlood>();
        InvokeRepeating(nameof(UpdateWaterLevel), 0, 0.02f / speed);
    }

    public void StopSimulation()
    {
        Playing = false;
        pumping = false;
        foreach (GameObject floodSector in floodSectorGameObjects) {
            floodSector.SetActive(false); 
        }
        SelectedFlood = floodsPerYear[SelectedFloodYear].ToList();
        aux = new List<WaterFlood>();
        CancelInvoke(nameof(UpdateWaterLevel));
    }
    void UpdateWaterLevel()
    {
        int time = 0;
        float chartHeight = 0;
        foreach (GameObject floodSector in floodSectorGameObjects)
        {
            var floodSectorData = SelectedFlood.FirstOrDefault(f => f.SectorId.Equals(floodSector.name));
            if(floodSectorData == null)
            {
                SelectedFlood = aux;
                return;
            }
            float newHeight = SelectedPumpCasier == floodSector.name ? floodSectorData.Level.Value - (0 * 2 / areaCasier[floodSector.name]) : floodSectorData.Level.Value;

            if(SelectedPumpCasier == floodSector.name)
                chartHeight = newHeight;

            Transform floodSectorTransform = floodSector.transform.GetChild(0).transform;
            floodSectorTransform.position = new Vector3(floodSectorTransform.position.x, newHeight, floodSectorTransform.position.z);

            time = floodSectorData.Time.Value;

            aux.Add(floodSectorData);
            SelectedFlood.Remove(floodSectorData);
        }
        UIManager UIManager = GameObject.Find("UI").GetComponent<UIManager>();
        Serie serie = UIManager.LineChart.series[0];
        if(serie.dataCount > 500)
        {
            for (int i = 0; i < serie.dataCount - 500; i++)
                serie.RemoveData(i);
        }
        serie.AddXYData(time, Math.Round(chartHeight, 3));
    }

    void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(GameObject.FindGameObjectWithTag("BuildingInfo"));
            ActiveBuildingInfo = null;
        }
    }
}
