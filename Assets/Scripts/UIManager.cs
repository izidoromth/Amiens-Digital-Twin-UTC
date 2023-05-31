using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button OpenCloseParameters;
    public TextMeshProUGUI ParametersLabel;
    public GameObject Parameters;
    public Button PlayButton;
    public Toggle Decennale;
    public Toggle Millenale;
    public Toggle PumpToggle;
    public Slider PumpSlider;
    public Button Speed2x;
    public Button Speed5x;
    public Button Speed10x;

    int selectedSpeed;
    private void Start()
    {
        OpenCloseParameters.onClick.AddListener(delegate () { OpenCloseParametersClicked(); });
        Millenale.onValueChanged.AddListener(delegate (bool val) { MillenaleSelected(val); });
        Decennale.onValueChanged.AddListener(delegate (bool val) { DecennaleSelected(val); });
        Speed2x.onClick.AddListener(delegate () { SpeedSelected(2); });
        Speed5x.onClick.AddListener(delegate () { SpeedSelected(5); });
        Speed10x.onClick.AddListener(delegate () { SpeedSelected(10); });
        PlayButton.onClick.AddListener(delegate () { PlaySimulation(); });
        PumpToggle.onValueChanged.AddListener(delegate (bool val) { PumpValueChanged(val); });
        PumpSlider.minValue = 100;
        PumpSlider.maxValue = 500;
    }

    void Update()
    {

    }

    void OpenCloseParametersClicked()
    {
        ParametersLabel.text = Parameters.activeSelf ? ">" : "<";
        Parameters.SetActive(!Parameters.activeSelf);
    }

    void MillenaleSelected(bool val)
    {
        if (val && Decennale.isOn)
        {
            Decennale.isOn = false;
        }

        if (val)
            GameObject.Find("TwinManager").GetComponent<TwinManager>().SelectFlood(9999);

        PlayButton.image.color = selectedSpeed != 0 && (Millenale.isOn || Decennale.isOn) ? new Color(.12f, .78f, .51f) : new Color(.82f, .82f, .82f);
    }

    void DecennaleSelected(bool val)
    {
        if (val && Millenale.isOn)
        {
            Millenale.isOn = false;
        }

        if(val)
            GameObject.Find("TwinManager").GetComponent<TwinManager>().SelectFlood(1994);

        PlayButton.image.color = selectedSpeed != 0 && (Millenale.isOn || Decennale.isOn) ? new Color(.12f, .78f, .51f) : new Color(.82f, .82f, .82f);
    }

    void PumpValueChanged(bool val)
    {
        GameObject.Find("TwinManager").GetComponent<TwinManager>().PumpsEnabled = val;
    }

    void SpeedSelected(int speed)
    {
        selectedSpeed = selectedSpeed == speed ? 0 : speed;

        PlayButton.image.color = selectedSpeed != 0 && (Millenale.isOn || Decennale.isOn) ? new Color(.12f, .78f, .51f) : new Color(.82f, .82f, .82f);

        if (selectedSpeed == 0)
        {
            Speed2x.image.color = new Color(0.81f, 0.81f, 0.81f);
            Speed5x.image.color = new Color(0.81f, 0.81f, 0.81f);
            Speed10x.image.color = new Color(0.81f, 0.81f, 0.81f);
        }
        else if (selectedSpeed == 2)
        {
            Speed2x.image.color = new Color(0.38f, 0.68f, 0.98f);
            Speed5x.image.color = new Color(0.81f, 0.81f, 0.81f);
            Speed10x.image.color = new Color(0.81f, 0.81f, 0.81f);
        }
        else if (selectedSpeed == 5)
        {
            Speed2x.image.color = new Color(0.81f, 0.81f, 0.81f);
            Speed5x.image.color = new Color(0.38f, 0.68f, 0.98f);
            Speed10x.image.color = new Color(0.81f, 0.81f, 0.81f);
        }
        else if (selectedSpeed == 10)
        {
            Speed2x.image.color = new Color(0.81f, 0.81f, 0.81f);
            Speed5x.image.color = new Color(0.81f, 0.81f, 0.81f);
            Speed10x.image.color = new Color(0.38f, 0.68f, 0.98f);
        }
    }

    void PlaySimulation()
    {
        TwinManager manager = GameObject.Find("TwinManager").GetComponent<TwinManager>();
        if (selectedSpeed != 0 && (Millenale.isOn || Decennale.isOn))
        {
            OpenCloseParametersClicked();            
            manager.Playing = true;
            manager.PlaySimulation(selectedSpeed);
        }
        else if (manager.Playing)
        {
            manager.Playing = false;
            manager.StopSimulation();
        }
        ModifyPlayButtonProperties(manager.Playing);
    }

    void ModifyPlayButtonProperties(bool playing)
    {
        if (playing)
            PlayButton.image.color = new Color(1f, 0f, 0f);
        else
            PlayButton.image.color = selectedSpeed != 0 && (Millenale.isOn || Decennale.isOn) ? new Color(.12f, .78f, .51f) : new Color(.82f, .82f, .82f);
    }
}
