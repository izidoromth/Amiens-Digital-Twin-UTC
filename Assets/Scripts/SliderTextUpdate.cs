using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextUpdate : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI text;
    void Start()
    {
        slider.onValueChanged.AddListener(delegate (float val) { OnChange(val); });
    }
    void OnChange(float val)
    {
        text.text = val.ToString("0");
    }
}
