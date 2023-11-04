using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_FType_ScrollView : MonoBehaviour, IBeginDragHandler
{
    [SerializeField] private UI_FurnitureCustomize ui_FurnitureCustomize;
    public void OnBeginDrag(PointerEventData eventData){
        ui_FurnitureCustomize.CloseSelectionCanvas();
    }
}
