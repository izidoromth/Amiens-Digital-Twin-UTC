using Assets.Scripts;
using DatabaseConnection.Entities;
using System.Linq;
using UnityEngine;

public class HandleBuildingInfo : MonoBehaviour
{
    TwinManager manager;
    bool flooding;
    string floodSectorName;
    SetBuildingInfo setBuildingInfoComponent;
    public BuildingNoGeom BuildingInfo;
    void Update()
    {
        if (manager == null)
            manager = GameObject.Find("TwinManager").GetComponent<TwinManager>();

        if (UIUtils.MouseoverObject(gameObject))
        {
            Debug.Log($"[{System.DateTime.Now}] ADTLog: {BuildingInfo.Id}");

            GetComponent<Outline>().enabled = true;

            if (Input.GetMouseButtonDown(0))
            {                
                Destroy(GameObject.FindGameObjectWithTag("BuildingInfo"));
                GameObject buildingInfo = Instantiate((GameObject)Resources.Load("Prefabs/BuildingInfo", typeof(GameObject)));
                buildingInfo.SetActive(false);
                buildingInfo.transform.SetParent(GameObject.Find("UI").transform, false);
                setBuildingInfoComponent = buildingInfo.GetComponent<SetBuildingInfo>();
                setBuildingInfoComponent.Nature = BuildingInfo.Nature;
                setBuildingInfoComponent.Usage = BuildingInfo.Usage1;
                setBuildingInfoComponent.Logts = BuildingInfo.NbLogts;
                setBuildingInfoComponent.Floors = BuildingInfo.NbEtages.ToString();
                setBuildingInfoComponent.Height = BuildingInfo.Hauter.ToString();
                setBuildingInfoComponent.ZMin = BuildingInfo.ZMinSol.ToString();
                setBuildingInfoComponent.FloodHeight = "0";
                buildingInfo.SetActive(true);
                manager.ActiveBuildingInfo = this;
            }
        }
        else if (manager.ActiveBuildingInfo != this)
        {
            GetComponent<Outline>().enabled = false;
        }

        if (flooding && setBuildingInfoComponent is not null && manager.SelectedFlood.FirstOrDefault(f => f.SectorId.Equals(floodSectorName)) is not null)
        {
            float minY = transform.position.y;
            float floodHeight = manager.SelectedFlood.FirstOrDefault(f => f.SectorId.Equals(floodSectorName)).Level.Value - minY;
            setBuildingInfoComponent.FloodHeight = floodHeight.ToString("0.00");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        flooding = true;
        floodSectorName = other.transform.gameObject.name;
    }
    void OnTriggerExit(Collider other)
    {
        flooding = false;
        if (setBuildingInfoComponent is not null)
            setBuildingInfoComponent.FloodHeight = "0";
    }
}
