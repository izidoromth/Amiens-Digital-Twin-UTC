using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandleFloodSectorInfo : MonoBehaviour
{
    Outline outline;
    TwinManager twinManager;
    void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineColor = new Color32(0x00, 0xF0, 0xFF, 0x19);
        outline.OutlineMode = Outline.Mode.OutlineAndSilhouette;
        outline.enabled = false;
        twinManager = GameObject.Find("TwinManager").GetComponent<TwinManager>();
    }

    void Update()
    {
        outline.enabled = UIUtils.MouseoverObject(gameObject) || twinManager.SelectedPumpCasier == gameObject.name;
    }

    void OnGUI()
    {
        var position = Camera.main.WorldToScreenPoint(gameObject.GetComponent<Renderer>().bounds.center);
        GUI.skin.label.fontSize = 25;
        var textSize = GUI.skin.label.CalcSize(new GUIContent(gameObject.name));
        GUI.Label(new Rect(position.x, Screen.height - position.y, textSize.x, textSize.y), gameObject.name);
    }
}
