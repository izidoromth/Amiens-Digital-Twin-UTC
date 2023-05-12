using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    public UnityEngine.UI.Slider Timeline;
    public TextMeshProUGUI Timelabel;
    public Component ClockScript;
    public Button OpenCloseParameters;
    public TextMeshProUGUI ParametersLabel;
    public GameObject Parameters;

    private void Start()
    {
        OpenCloseParameters.onClick.AddListener(delegate () { OpenCloseParametersClicked(); });
    }

    void Update()
    {
        Timeline.value += 0.01f; // *(ClockScript as Clock).timeSpeed;
        var timeInSeconds = Timeline.value;
        Timelabel.text = Mathf.Floor(timeInSeconds).ToString("00") + "h" + Mathf.FloorToInt((timeInSeconds % 1)*60).ToString("00");
        if (Timeline.value >= Timeline.maxValue) {
            Timeline.value = 0;
        }
    }

    void OpenCloseParametersClicked()
    {
        ParametersLabel.text = Parameters.activeSelf ? ">" : "<";
        Parameters.SetActive(!Parameters.activeSelf);
    }
}
