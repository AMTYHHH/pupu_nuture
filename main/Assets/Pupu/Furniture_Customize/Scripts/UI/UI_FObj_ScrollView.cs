using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_FObj_ScrollView : MonoBehaviour, IBeginDragHandler
{
    public void OnBeginDrag(PointerEventData eventData){
        EventHandler.Call_UI_OnDragObjectSelectionView();
    }
}