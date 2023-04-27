using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    float timeSpeed = 360.0f;
    int hoursPassed = 0;
    int lastTimepoint = 0;
    float timeFixed = 0;

    void Update()
    {
        timeFixed = Time.realtimeSinceStartup * timeSpeed;
        if (timeFixed % (60*60) < 1 && lastTimepoint !=  (int)timeFixed)
        {
            lastTimepoint = (int)(Time.realtimeSinceStartup * timeSpeed);
            Debug.Log(++hoursPassed);
        }
    }
}
