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
        if (selectedSpeed != 0 && (Millenale.isOn || Decennale.isOn))
        {
            OpenCloseParametersClicked();
            GameObject.Find("TwinManager").GetComponent<TwinManager>().PlaySimulation(selectedSpeed);
        }
    }
}
