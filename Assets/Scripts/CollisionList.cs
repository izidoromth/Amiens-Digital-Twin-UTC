using System.Collections.Generic;    // Don't forget to add this if using a List.
using UnityEngine;

public class CollisionList : MonoBehaviour
{

    // Declare and initialize a new List of GameObjects called currentCollisions.
    List<GameObject> currentCollisions = new List<GameObject>();

    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        currentCollisions.Add(col.gameObject);

        // Print the entire list to the console.
        foreach (GameObject gObject in currentCollisions)
        {
            print(gObject.name);
        }
    }

    void OnTriggerExit(Collider col)
    {

        // Remove the GameObject collided with from the list.
        currentCollisions.Remove(col.gameObject);

        // Print the entire list to the console.
        foreach (GameObject gObject in currentCollisions)
        {
            print(gObject.name);
        }
    }
}