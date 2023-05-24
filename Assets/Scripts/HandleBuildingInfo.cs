using Assets.Scripts;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandleBuildingInfo : MonoBehaviour
{
    TwinManager manager;
    bool flooding;
    string floodSectorName;
    SetBuildingInfo setBuildingInfoComponent;

    public Building BuildingInfo;
    void Update()
    {
        if(manager == null)
            manager = GameObject.Find("TwinManager").GetComponent<TwinManager>();

        if (MouseoverObject())
        {
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
                setBuildingInfoComponent.Logts = BuildingInfo.NbLogts as string;
                setBuildingInfoComponent.Floors = BuildingInfo.NbFloors.ToString();
                setBuildingInfoComponent.Height = BuildingInfo.Height.ToString();
                setBuildingInfoComponent.ZMin = BuildingInfo.ZMinSol.ToString();
                setBuildingInfoComponent.FloodHeight = "0";
                    buildingInfo.SetActive(true);
                manager.ActiveBuildingInfo = this;
            }
        }
        else if(manager.ActiveBuildingInfo != this)
        {
            GetComponent<Outline>().enabled = false;
        }

        if (flooding && setBuildingInfoComponent is not null && FloodDataInterface.Instance.FloodsPerYear[9999].FirstOrDefault(f => f.SectorId.Equals(floodSectorName)) is not null)
        {
            float floodHeight = FloodDataInterface.Instance.FloodsPerYear[9999].FirstOrDefault(f => f.SectorId.Equals(floodSectorName)).Height - (float)BuildingInfo.ZMinSol;
            floodHeight = floodHeight < 0 ? 0 : floodHeight;
            setBuildingInfoComponent.FloodHeight = floodHeight.ToString("0.00");
        }


    }

    bool MouseoverObject()
    {
        RaycastHit hit;
        Ray ray = FindAnyObjectByType<Camera>().ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit) && hit.collider.gameObject.Equals(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        flooding = true;
        floodSectorName = other.transform.gameObject.name;
    }
    void OnTriggerExit(Collider other)
    {
        flooding = false;
    }
}
