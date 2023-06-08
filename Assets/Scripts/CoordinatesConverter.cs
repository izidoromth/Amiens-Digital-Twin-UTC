using System;
using UnityEngine;

public class CoordinatesConv : MonoBehaviour
{
    public float unitsDegree;

    public float latitude;

    public float longitude;

    public float referenceLatitude;

    public float referenceLongitude;

    void Start()
    {
        transform.position = new Vector3(((longitude - referenceLongitude) * unitsDegree), transform.position.y, ((latitude - referenceLatitude) * unitsDegree));
    }
}

[Serializable]
public class Coordinates
{

    public double Latitude;

    [SerializeField]
    public double Longitude;
}
