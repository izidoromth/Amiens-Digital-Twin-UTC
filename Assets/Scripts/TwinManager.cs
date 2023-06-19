using DatabaseConnection.Context;
using DatabaseConnection.Entities;
using Dummiesman;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using XCharts.Runtime;

public class TwinManager : MonoBehaviour
{
    GameObject replacedBuilding;
    List<GameObject> buildingsNew;
    List<GameObject> floodSectorGameObjects = new List<GameObject>();
    List<WaterFlood> aux = new List<WaterFlood>();
    Dictionary<int, List<WaterFlood>> floodsPerYear = new Dictionary<int, List<WaterFlood>>();
    bool alertEnabled;

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

    Dictionary<int, Dictionary<string, float>> minCasier = new Dictionary<int, Dictionary<string, float>>();

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

    public GameObject StockageTerrain;
    public GameObject MainTerrain;
    public GameObject BatardeauxOn;
    public GameObject BatardeauxOff;

    public AmiensDigitalTwinDbContext Context;

    public List<WaterFlood> SelectedFlood;

    public Component ActiveBuildingInfo;
    public bool Playing { get; set; }
    public bool Paused { get; set; } = false;
    public float FloodThreshold { get; set; } = 0.25f;
    public int SelectedFloodYear { get; set; }
    public string SelectedPumpCasier { get; set; } = "Non";
    public float PumpValue { get; set; } = 100;

    void Start()
    {
        // Connect to the database
        try
        {
            Context = DbConnectionContext.GetContext<AmiensDigitalTwinDbContext>(
            (options, defaultSchema) => { return new AmiensDigitalTwinDbContext(options, defaultSchema); },
            "postgres", "12345678", "amiens_digital_twin", port: "5433");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        floodsPerYear.Add(1994, Context.WaterFloods.Where(f => f.Year == 1994).OrderBy(f => f.Time).ToList());
        floodsPerYear.Add(9999, Context.WaterFloods.Where(f => f.Year == 9999 && f.Time > 2250).OrderBy(f => f.Time).ToList());

        Dictionary<string, float> auxDict = new Dictionary<string, float>()
        {
            { "KE02", 0f },
            { "KE03", 0f },
            { "KE06", 0f },
            { "KE07", 0f },
            { "KE08", 0f },
            { "KE10", 0f },
            { "KE11", 0f },
            { "KE12", 0f },
            { "KE14", 0f },
            { "KF00", 0f },
            { "KF01", 0f },
            { "KF02", 0f },
            { "KF03", 0f }
        };

        foreach (KeyValuePair<int, List<WaterFlood>> entry in floodsPerYear)
        {
            foreach (KeyValuePair<string, float> entry2 in new Dictionary<string, float>(auxDict))
            {
                auxDict[entry2.Key] = floodsPerYear[entry.Key].Where(f => f.SectorId == entry2.Key).Min(f => f.Level.Value);
            }
            minCasier.Add(entry.Key, auxDict);
        }

        // Instantiate city objects
        InstantiateFloodSectors();
        InstantiateBuildings();
        //InstantiateTerrains();

        Debug.Log($"[{System.DateTime.Now}] ADTLog: Loading finished successfully");
    }

    void Update()
    {
        HandleInputs();
    }

    void InstantiateFloodSectors()
    {
        foreach (FloodSector sector in Context.FloodSectors.ToList())
        {
            GameObject floodSectorGameObject = LoadFromGeometry(sector.Geometry, sector.SectorId);

            floodSectorGameObject.transform.tag = "FloodSector";

            floodSectorGameObjects.Add(floodSectorGameObject);

            floodSectorGameObject.SetActive(false);

            // add mesh collider to detect flood
            GameObject floodSector = floodSectorGameObject.transform.GetChild(0).gameObject;
            floodSector.AddComponent<MeshCollider>();
            floodSector.AddComponent<HandleFloodSectorInfo>();

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
        buildingsNew = new List<GameObject>(GameObject.FindGameObjectsWithTag("BuildingsNew"));
        foreach(GameObject building in buildingsNew) building.SetActive(false);

        foreach (BuildingNoGeom building in Context.BuildingsNoGeom.ToList())
        {
            try
            {
                GameObject buildingGameObject = GameObject.Find(building.Id);
                if (buildingGameObject == null)
                    continue;

                if (buildingGameObject.name == "BATIMENT0000000224110574")
                    replacedBuilding = buildingGameObject;

                buildingGameObject.SetActive(false);

                Outline outline = buildingGameObject.AddComponent<Outline>();
                outline.enabled = false;
                outline.OutlineWidth = 8;

                HandleBuildingInfo buildingInfoComponent = buildingGameObject.AddComponent<HandleBuildingInfo>();
                buildingInfoComponent.BuildingInfo = building; 
                if (buildingGameObject.name.Contains("3454128"))
                {
                    buildingGameObject.SetActive(true);
                    continue;
                }
                MeshCollider buildingMeshCollider = buildingGameObject.AddComponent<MeshCollider>();
                Rigidbody buildingRigidbody = buildingGameObject.AddComponent<Rigidbody>();
                buildingRigidbody.useGravity = false;
                buildingRigidbody.isKinematic = true;
                buildingMeshCollider.convex = true;
                buildingMeshCollider.isTrigger = true;

                buildingGameObject.SetActive(true);                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    void InstantiateTerrains()
    {
        foreach (DatabaseConnection.Entities.Terrain terrain in Context.Terrains.ToList())
        {
            LoadFromGeometry(terrain.Geometry, terrain.Id.ToString(), "Terrain.png");
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

    public void PauseOrResumeSimulation(int speed)
    {
        if(!Paused)
            CancelInvoke(nameof(UpdateWaterLevel));
        else
            InvokeRepeating(nameof(UpdateWaterLevel), 0, 0.02f / speed);

        Paused = !Paused;
    }

    public void StopSimulation()
    {
        Playing = Paused = false;
        GameObject.Find("UI").GetComponent<UIManager>().DisableAlert();
        foreach (GameObject floodSector in floodSectorGameObjects)
        {
            floodSector.SetActive(false);
        }
        foreach(GameObject building in GameObject.FindGameObjectsWithTag("Buildings"))
        {
            ExecuteEvents.Execute<ICustomMessageTarget>(building, null, (x, y) => x.ResetColor());
        }
        SelectedFlood = floodsPerYear[SelectedFloodYear].ToList();
        aux = new List<WaterFlood>();
        CancelInvoke(nameof(UpdateWaterLevel));
    }

    public void ChangeAmenagementState(bool future)
    {
        foreach(GameObject building in buildingsNew) building.SetActive(future);
        replacedBuilding.SetActive(!future);
    }

    public void ChangeTerrainState(bool stockage)
    {
        MainTerrain.SetActive(!stockage);
        StockageTerrain.SetActive(stockage);
    }

    public void EnableBatardeaux(bool enabled)
    {
        BatardeauxOff.SetActive(!enabled);
        BatardeauxOn.SetActive(enabled);
    }

    void UpdateWaterLevel()
    {
        int time = 0;
        double avgHeight = 0;

        UIManager uiManager = GameObject.Find("UI").GetComponent<UIManager>();

        foreach (GameObject floodSector in floodSectorGameObjects)
        {
            // retrieve water level data
            var floodSectorData = SelectedFlood.FirstOrDefault(f => f.SectorId.Equals(floodSector.name));
            if (floodSectorData == null)
            {
                SelectedFlood = aux.ToList();
                uiManager.PlayOrStopSimulation();
                uiManager.Parameters.SetActive(true);
                uiManager.OpenCloseParametersClicked();
                StopSimulation();
                aux.Clear();
                break;
            }

            // calculate water height taking out terrain level
            float currHeight = floodSectorData.Level.Value - minCasier[SelectedFloodYear][floodSector.name];
            bool thresholdReached = SelectedPumpCasier == floodSector.name && currHeight > FloodThreshold;

            // calculate new height based if threshold reached
            float noise = 0.01f*Mathf.Sin(0.005f*floodSectorData.Time.Value);
            float newHeight = thresholdReached ? FloodThreshold + noise : currHeight;

            avgHeight += newHeight;

            // update sector height
            Transform floodSectorTransform = floodSector.transform.GetChild(0).transform;
            floodSectorTransform.position = new Vector3(floodSectorTransform.position.x, newHeight + fondCasier[floodSector.name], floodSectorTransform.position.z);

            // show water level alert
            if(SelectedPumpCasier == floodSector.name && thresholdReached && !alertEnabled)
            {
                uiManager.ShowAlert();
                alertEnabled = true;
            }
            else if (SelectedPumpCasier == floodSector.name && !thresholdReached && alertEnabled)
            {
                uiManager.DisableAlert();
                alertEnabled = false;
            }

            // update chart
            time = floodSectorData.Time.Value; 
            if (SelectedPumpCasier == floodSector.name)
            {
                uiManager.AddSeriesData(time, Math.Round(newHeight, 3));
            }

            // update water level list and aux list
            aux.Add(floodSectorData);
            SelectedFlood.Remove(floodSectorData);
        }

        if(SelectedPumpCasier == "Non")
        {
            uiManager.AddSeriesData(time, Math.Round(avgHeight / floodSectorGameObjects.Count, 3));
        }

        if(aux.Count == 0)
            uiManager.ClearChartData();
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
