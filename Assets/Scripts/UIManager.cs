using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;
using XCharts.Runtime;

public class UIManager : MonoBehaviour
{
    public Button OpenCloseParameters;
    public TextMeshProUGUI ParametersLabel;
    public GameObject Parameters;
    public Button PlayButton;
    public Toggle Decennale;
    public Toggle Millenale;
    public Toggle Actuel;
    public Toggle Future;
    public Button Speed2x;
    public Button Speed5x;
    public Button Speed10x;
    public TMP_Dropdown CasiersDropdown;
    public Slider ThresholdSlider;
    public TextMeshProUGUI PumpLabel;
    public Button CloseApp;

    TwinManager manager;
    int selectedSpeed;

    public GameObject WaterLevelLineChart;
    LineChart lineChart;

    private void Start()
    {
        manager = GameObject.Find("TwinManager").GetComponent<TwinManager>();

        OpenCloseParameters.onClick.AddListener(delegate () { OpenCloseParametersClicked(); });
        Millenale.onValueChanged.AddListener(delegate (bool val) { MillenaleSelected(val); });
        Decennale.onValueChanged.AddListener(delegate (bool val) { DecennaleSelected(val); });
        Actuel.onValueChanged.AddListener(delegate (bool val) { ActuelSelected(val); });
        Future.onValueChanged.AddListener(delegate (bool val) { FutureSelected(val); });
        Speed2x.onClick.AddListener(delegate () { SpeedSelected(2); });
        Speed5x.onClick.AddListener(delegate () { SpeedSelected(5); });
        Speed10x.onClick.AddListener(delegate () { SpeedSelected(10); });
        PlayButton.onClick.AddListener(delegate () { PlaySimulation(); });
        CloseApp.onClick.AddListener(delegate () { Application.Quit(); });
        ThresholdSlider.onValueChanged.AddListener(delegate (float val) { ThresholdChanged(val); });
        List<TMP_Dropdown.OptionData> casierOptions = new List<TMP_Dropdown.OptionData>
        {
            new TMP_Dropdown.OptionData() { text = "Non" }
        };
        foreach (var sector in manager.Context.FloodSectors.Select(s => s.SectorId).ToList())
            casierOptions.Add(new TMP_Dropdown.OptionData() { text = sector });
        CasiersDropdown.options = casierOptions;
        CasiersDropdown.onValueChanged.AddListener(delegate (int val) { CasierSelected(val); });
    }

    public void ClearChartData()
    {
        lineChart.RemoveAllSerie();
        lineChart.AddSerie<Line>();
    }

    GameObject alert;
    public void ShowAlert()
    {
        alert = Instantiate((GameObject)Resources.Load("Prefabs/FloodAlert", typeof(GameObject)));
        alert.transform.SetParent(GameObject.Find("UI").transform, false);
    }

    public void DisableAlert()
    {
        Destroy(alert);
    }

    public void AddSeriesData(int x, double y) => lineChart.series[0]?.AddXYData(x, y);

    void CreateWaterLevelLineChart()
    {
        WaterLevelLineChart = new GameObject("WaterLevelLineChart");

        // init line chart
        lineChart = WaterLevelLineChart.AddComponent<LineChart>();
        lineChart.Init();
        lineChart.RemoveAllSerie();
        lineChart.AddSerie<Line>();

        lineChart.GetChartComponent<Title>().text = "Average Water Level (m)";
        lineChart.GetChartComponent<GridCoord>().left = .2f;
        lineChart.GetChartComponent<XAxis>().type = Axis.AxisType.Value;
        lineChart.GetChartComponent<XAxis>().minMaxType = Axis.AxisMinMaxType.MinMax;
        lineChart.GetChartComponent<YAxis>().type = Axis.AxisType.Value;
        lineChart.GetChartComponent<YAxis>().minMaxType = Axis.AxisMinMaxType.MinMax;

        // adjust size and position
        WaterLevelLineChart.transform.SetParent(GameObject.Find("UI").transform, false);
        RectTransform transform = WaterLevelLineChart.GetComponent<RectTransform>();
        transform.localPosition = new Vector2(715, 375);
        transform.sizeDelta = new Vector2(400, 250);

        Instantiate(WaterLevelLineChart);
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
            manager.SelectedFloodYear = 9999;

        PlayButton.image.color = selectedSpeed != 0 && (Millenale.isOn || Decennale.isOn) ? new Color(.12f, .78f, .51f) : new Color(.82f, .82f, .82f);
    }

    void DecennaleSelected(bool val)
    {
        if (val && Millenale.isOn)
        {
            Millenale.isOn = false;
        }

        if (val)
            manager.SelectedFloodYear = 1994;

        PlayButton.image.color = selectedSpeed != 0 && (Millenale.isOn || Decennale.isOn) ? new Color(.12f, .78f, .51f) : new Color(.82f, .82f, .82f);
    }

    void ActuelSelected(bool val)
    {
        if (val && Actuel.isOn)
        {
            Future.isOn = false;
        }
    }

    void FutureSelected(bool val)
    {
        if (val && Future.isOn)
        {
            Actuel.isOn = false;
        }
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

    void ThresholdChanged(float val)
    {
        manager.FloodThreshold = (float)Math.Round(val, 1);
        PumpLabel.text = $"{manager.FloodThreshold}m";
    }

    void CasierSelected(int val)
    {
        manager.SelectedPumpCasier = CasiersDropdown.options[val].text;
    }

    void PlaySimulation()
    {
        if (!manager.Playing && selectedSpeed != 0 && (Millenale.isOn || Decennale.isOn))
        {
            manager.PlaySimulation(selectedSpeed);
            CreateWaterLevelLineChart();
            OpenCloseParametersClicked();
            PlayButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Finir la simulation";
            ChangeComponentsInteractable(false);
        }
        else if (manager.Playing)
        {
            manager.StopSimulation();
            Destroy(WaterLevelLineChart.gameObject);
            PlayButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Lancer la simulation";
            ChangeComponentsInteractable(true);
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

    void ChangeComponentsInteractable(bool state)
    {
        Millenale.interactable = state;
        Decennale.interactable = state;
        Actuel.interactable = state;
        Future.interactable = state;
        Speed2x.interactable = state;
        Speed5x.interactable = state;
        Speed10x.interactable = state;
        ThresholdSlider.interactable = state;
        CasiersDropdown.interactable = state;
    }
}
