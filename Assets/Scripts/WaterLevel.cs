using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(0, 80 + 50f * Mathf.Sin(.2f * Time.realtimeSinceStartup), 0);        
    }
}
