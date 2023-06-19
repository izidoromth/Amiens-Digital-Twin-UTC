using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImagePointer : MonoBehaviour, IPointerClickHandler
{

    public event EventHandler OnPointerClickEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClickEvent?.Invoke(this, null);
    }
}
