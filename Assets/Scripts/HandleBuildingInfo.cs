using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBuildingInfo : MonoBehaviour
{
    public Building BuildingInfo;
    void Update()
    {
        TwinManager manager = GameObject.Find("TwinManager").GetComponent<TwinManager>();
        if (MouseoverObject())
        {
            GetComponent<Outline>().enabled = true;
            
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(GameObject.FindGameObjectWithTag("BuildingInfo"));
                GameObject buildingInfo = Instantiate((GameObject)Resources.Load("Prefabs/BuildingInfo", typeof(GameObject)));
                buildingInfo.SetActive(false);
                buildingInfo.transform.SetParent(GameObject.Find("UI").transform, false);
                SetBuildingInfo setBuildingInfoComponent = buildingInfo.GetComponent<SetBuildingInfo>();
                setBuildingInfoComponent.Nature = BuildingInfo.Nature;
                setBuildingInfoComponent.Usage = BuildingInfo.Usage1;
                setBuildingInfoComponent.Logts = BuildingInfo.NbLogts as string;
                setBuildingInfoComponent.Floors = BuildingInfo.NbFloors.ToString();
                setBuildingInfoComponent.Height = BuildingInfo.Height.ToString();
                setBuildingInfoComponent.ZMin = BuildingInfo.ZMinSol.ToString();
                buildingInfo.SetActive(true);
                manager.ActiveBuildingInfo = this;
            }
        }
        else if(manager.ActiveBuildingInfo != this)
        {
            GetComponent<Outline>().enabled = false;
        }
    }

    bool MouseoverObject()
    {
        RaycastHit hit;
        Ray ray = FindAnyObjectByType<Camera>().ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit) && hit.collider.gameObject.Equals(gameObject);
    }
}
