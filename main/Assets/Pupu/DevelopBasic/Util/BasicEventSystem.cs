using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A basic C# Event System
public static class EventHandler
{
#region GameBasic Event
    public static event Action E_BeforeUnloadScene;
    public static void Call_BeforeUnloadScene(){E_BeforeUnloadScene?.Invoke();}
    public static event Action E_AfterLoadScene;
    public static void Call_AfterLoadScene(){E_AfterLoadScene?.Invoke();}
    public static event Action E_OnClickOnNonUI;
    public static void Call_OnClickOnNonUI(){E_OnClickOnNonUI?.Invoke();}
#endregion

    public static event Action<bool> E_OnPlayerInViewChange;
    public static void Call_OnPlayerInViewChange(bool isInView)=>E_OnPlayerInViewChange?.Invoke(isInView);

    public static event Action E_OnFindPlayer;
    public static void Call_OnFindPlayer()=>E_OnFindPlayer?.Invoke();

    public static event Action<Furniture_Base> E_OnPlacedFurniture_Changed;
    public static void Call_OnPlacedFurniture_Changed(Furniture_Base furniture)=>E_OnPlacedFurniture_Changed?.Invoke(furniture);

#region Furniture UI Event
    public static event Action<string, FurnitureType, FurnitureLayer, bool> E_UI_OnSelectObject; //当玩家选择菜单中的某件家具时，呼叫该消息，当操作为回收家具时，bool为true
    public static void Call_UI_OnSelectObject(string fName, FurnitureType fType, FurnitureLayer fLayer, bool isTakeBack)=>E_UI_OnSelectObject?.Invoke(fName, fType, fLayer, isTakeBack);
    public static event Action<string, FurnitureType, FurnitureLayer> E_UI_OnDragObject; //当玩家拖动菜单中的某件家具时，呼叫该消息
    public static void Call_UI_OnDragObject(string fName, FurnitureType fType, FurnitureLayer fLayer)=>E_UI_OnDragObject?.Invoke(fName, fType, fLayer);
    public static event Action E_UI_OnDragObjectSelectionView;
    public static void Call_UI_OnDragObjectSelectionView()=>E_UI_OnDragObjectSelectionView?.Invoke();
    public static event Action<FurnitureLayer> E_UI_OnSelectFurnitureLayer;
    public static void Call_UI_OnSelectFurnitureLayer(FurnitureLayer fLayer)=>E_UI_OnSelectFurnitureLayer?.Invoke(fLayer);
#endregion
}