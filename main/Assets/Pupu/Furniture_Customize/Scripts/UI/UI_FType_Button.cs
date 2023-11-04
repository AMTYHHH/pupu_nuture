using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_FType_Button : MonoBehaviour, IDeselectHandler
{
    [SerializeField] private FurnitureType furniture_Type;
    [SerializeField] private Image canvas_bridge;
    [SerializeField] private UI_FurnitureCustomize furnitureCustomize;
    private Button m_button;
    void Awake(){
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnClick);
    }
    void OnDestroy()=>m_button.onClick.RemoveListener(OnClick);

    public void OnClick(){
        canvas_bridge.gameObject.SetActive(true);
        furnitureCustomize.OnSelectFurnitureType(furniture_Type);
    }
//IDeselectHandler的实现
    public void OnDeselect(BaseEventData eventData){
        canvas_bridge.gameObject.SetActive(false);
    }
}
