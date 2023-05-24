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
    public TextMeshProUGUI FloodHeightLabel;

    public string Nature;
    public string Usage;
    public string Logts;
    public string Floors;
    public string Height;
    public string ZMin;
    public string FloodHeight;
    void Start()
    {
        NatureLabel.text = $"Nature: {Nature}";
        UsageLabel.text = $"Usage: {Usage}";
        LogtsLabel.text = $"N° Logements: {Logts}";
        FloorsLabel.text = $"N° Etages: {Floors}";
        HeightLabel.text = $"Hauteur: {Height}m";
        ZMinLabel.text = $"Hauteur au sol: {ZMin}m";
    }

    void Update()
    {
        FloodHeightLabel.text = $"Hauteur d'inondation: {FloodHeight}m";
    }
}
