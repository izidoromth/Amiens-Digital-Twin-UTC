using FastObjUnity.Editor;
using FastObjUnity.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

public class ObjImport : MonoBehaviour
{
    void Start()
    {
        string objPath = "C:\\Users\\Matheus\\xicavirus.obj";

        GameObject obj = loadAndDisplayMesh(objPath);

        //Position it in front od=f the camera. Your ZOffset may be different
        Camera cam = Camera.main;
        float zOffset = 4f;
        obj.transform.position = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane + zOffset));
    }

    GameObject loadAndDisplayMesh(string path)
    {
        //Create new GameObject to hold it
        GameObject meshHolder = new GameObject("Loaded Mesh");

        //Load Mesh
        Mesh mesh = FastObjConverter.ImportFastObj(path)[0].Item2;
        //return null;

        //Attach Mesh Filter
        attachMeshFilter(meshHolder, mesh);

        //Create Material
        Material mat = createMaterial();

        //Attach Mesh Renderer
        attachMeshRenderer(meshHolder, mat);

        return meshHolder;
    }

    void attachMeshFilter(GameObject target, Mesh mesh)
    {
        MeshFilter mF = target.AddComponent<MeshFilter>();
        mF.mesh = mesh;
    }

    Material createMaterial()
    {
        Material mat = new Material(Shader.Find("Standard"));
        return mat;
    }

    void attachMeshRenderer(GameObject target, Material mat)
    {
        MeshRenderer mR = target.AddComponent<MeshRenderer>();
        mR.material = mat;
    }
}
