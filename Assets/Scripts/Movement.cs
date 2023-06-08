using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject Circuit;
    private List<Vector3> waypoints = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in Circuit.transform)
        {
            waypoints.Add(t.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToNext = Vector3.Distance(transform.position, waypoints[0]);

        // waypoint reached, send it to the end of the list
        if (distanceToNext < 3)
        {
            waypoints.Add(waypoints[0]);
            waypoints.Remove(waypoints[0]);
        }

        Vector3 directionToGo = (waypoints[0] - transform.position).normalized;
        transform.position += .05f * directionToGo;

        transform.rotation = Quaternion.FromToRotation(Vector3.forward, directionToGo);
    }
}
