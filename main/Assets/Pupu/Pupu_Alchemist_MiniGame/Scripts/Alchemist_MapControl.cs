using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Alchemist_MapControl : MonoBehaviour
{
[Header("Pupu部件选择")]
    [SerializeField] private InputAction press;
[Header("Pupu 位置")]
    [SerializeField] private Transform pupuMapTrans;

    private bool isOverUI;

    void OnEnable(){
        EventHandler.E_OnFindPlayer += OnFindPlayer_Handler;

        press.performed += OnPress;
        press.Enable();
    }
    void OnDisable(){
        EventHandler.E_OnFindPlayer -= OnFindPlayer_Handler;

        press.Disable();
        press.performed -= OnPress;
    }
    void Update()
    {
        isOverUI = EventSystem.current.IsPointerOverGameObject();
    }
    void OnFindPlayer_Handler(){
        pupuMapTrans.localPosition = Vector3.zero;
    }
    void OnPress(InputAction.CallbackContext callback){
        if(!isOverUI) EventHandler.Call_OnClickOnNonUI();
    }
}
