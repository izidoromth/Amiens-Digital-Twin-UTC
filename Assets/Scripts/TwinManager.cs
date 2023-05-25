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

public class TwinManager : MonoBehaviour
{
    List<GameObject> floodSectorGameObjects = new List<GameObject>();
    List<WaterFlood> aux = new List<WaterFlood>();
    public Dictionary<int, List<WaterFlood>> floodsPerYear = new Dictionary<int, List<WaterFlood>>();
    AmiensDigitalTwinDbContext context;

    public Component ActiveBuildingInfo;

    void Start()
    {
        // Connect to the database
        context = DbConnectionContext.GetContext<AmiensDigitalTwinDbContext>(
            (options, defaultSchema) => { return new AmiensDigitalTwinDbContext(options, defaultSchema); },
            "postgres", "12345678", "amiens_digital_twin", port: "5433");

        floodsPerYear = new Dictionary<int, List<WaterFlood>>();
        floodsPerYear.Add(9999, context.WaterFloods.Where(x => x.Year == 9999).OrderBy(x => x.Time).ToList());

        // Instantiate city objects
        InstantiateFloodSectors();
        InstantiateBuildings();
        InstantiateTerrains();

        // Flood tests
        InvokeRepeating("UpdateWaterLevel", 0, 0.02f);
    }

    void Update()
    {
        HandleInputs();
    }

    void InstantiateFloodSectors()
    {
        foreach(FloodSector sector in context.FloodSectors.ToList())
        {
            GameObject floodSectorGameObject = LoadFromGeometry(sector.Geometry, sector.SectorId, "Texture.jpg");

            floodSectorGameObjects.Add(floodSectorGameObject);

            floodSectorGameObject.SetActive(false);
            GameObject floodSector = floodSectorGameObject.transform.GetChild(0).gameObject;
            floodSector.AddComponent<MeshCollider>();
            floodSectorGameObject.SetActive(true);
        }
    }

    void InstantiateBuildings()
    {
        foreach (Building building in context.Buildings.ToList())
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
        foreach (DatabaseConnection.Entities.Terrain terrain in context.Terrains.ToList())
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

    void UpdateWaterLevel()
    {
        foreach(GameObject floodSector in floodSectorGameObjects)
        {
            var floodSectorData = floodsPerYear[9999].FirstOrDefault(f => f.SectorId.Equals(floodSector.name));
            if(floodSectorData == null)
            {
                floodsPerYear[9999] = aux;
                return;
            }
            Transform floodSectorTransform = floodSector.transform.GetChild(0).transform;
            floodSectorTransform.position = new Vector3(floodSectorTransform.position.x, floodSectorData.Level.Value, floodSectorTransform.position.z);
            aux.Add(floodSectorData);
            floodsPerYear[9999].Remove(floodSectorData);
        }
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
