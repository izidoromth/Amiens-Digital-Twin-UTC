using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBuildingInfo : MonoBehaviour
{
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
                buildingInfo.transform.SetParent(GameObject.Find("UI").transform, false);
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
