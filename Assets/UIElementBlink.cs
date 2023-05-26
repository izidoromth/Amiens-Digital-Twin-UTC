using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementBlink : MonoBehaviour
{
    const int speed = 3;
    float time = 0;

    CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        time = 0;
    }

    void Update()
    {
        canvasGroup.alpha = Mathf.Abs(Mathf.Sin(speed*time));
        time += Time.deltaTime;
    }
}
