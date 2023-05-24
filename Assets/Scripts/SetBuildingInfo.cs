using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetBuildingInfo : MonoBehaviour
{
    public TextMeshProUGUI NatureLabel;
    public TextMeshProUGUI UsageLabel;
    public TextMeshProUGUI LogtsLabel;
    public TextMeshProUGUI FloorsLabel;
    public TextMeshProUGUI HeightLabel;
    public TextMeshProUGUI ZMinLabel;

    public string Nature;
    public string Usage;
    public string Logts;
    public string Floors;
    public string Height;
    public string ZMin;
    void Start()
    {
        NatureLabel.text = $"Nature: {Nature}";
        UsageLabel.text = $"Usage: {Usage}";
        LogtsLabel.text = $"N Logts: {Logts}";
        FloorsLabel.text = $"N Etages: {Floors}";
        HeightLabel.text = $"Halteur: {Height}m";
        ZMinLabel.text = $"Halteur min: {ZMin}m";
    }
}
