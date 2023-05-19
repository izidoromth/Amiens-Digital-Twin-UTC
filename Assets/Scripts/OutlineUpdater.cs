using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineUpdater : MonoBehaviour
{
    void Update()
    {
        RaycastHit hit;
        Ray ray = FindAnyObjectByType<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.Equals(gameObject))
        {
            GetComponent<Outline>().enabled = true;
        }
        else
        {
            GetComponent<Outline>().enabled = false;
        }
    }
}
