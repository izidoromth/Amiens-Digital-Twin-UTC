using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTest : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Generated Mesh";

        mesh.vertices = GenerateVertices();
        mesh.triangles = GenerateTriangles();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    Vector3[] GenerateVertices()
    {
        return new Vector3[] 
        {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0),
        };
    }

    int[] GenerateTriangles()
    {
        return new int[]
        {
            1,0,2,
            1,2,3
        };
    }



    //void Update()
    //{
    //    Mesh mesh = GetComponent<MeshFilter>().mesh;
    //    Vector3[] vertices = mesh.vertices;
    //    Vector3[] normals = mesh.normals;

    //    for (var i = 0; i < vertices.Length; i++)
    //    {
    //        vertices[i] += normals[i] * Mathf.Sin(Time.time);
    //    }

    //    mesh.vertices = vertices;
    //}
}
