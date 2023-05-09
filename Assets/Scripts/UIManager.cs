using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    public Slider Timeline;

    void Update()
    {
        Timeline.value += 0.0001f;
        if(Timeline.value >= 1) {
            Timeline.value = 0;
        }
    }
}
