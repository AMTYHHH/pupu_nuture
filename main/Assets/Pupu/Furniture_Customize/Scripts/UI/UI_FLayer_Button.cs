using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_FLayer_Button : MonoBehaviour
{
    [SerializeField] private FurnitureLayer fLayer;
    [SerializeField] private UI_FurnitureCustomize ui_FurnitureCustomize;

    [SerializeField] private Color selectColor;
    [SerializeField] private Color deselectColor;

    [SerializeField] private CanvasGroup m_button_group;

    private Image buttonImage;

    void Awake(){
        buttonImage = GetComponent<Image>();
        m_button_group = GetComponent<CanvasGroup>();

        GetComponent<Button>().onClick.AddListener(OnClick);
    }
    void OnDestroy(){
        GetComponent<Button>().onClick.RemoveListener(OnClick);
    }
    public void OnClick()=>ui_FurnitureCustomize.SelectFurnitureLayer(fLayer, this);
    public void Selected(){
        buttonImage.color = selectColor;
    }
    public void Deselected(){
        buttonImage.color = deselectColor;
    }
    public void SwitchButton(bool isActive){
        m_button_group.alpha = isActive?1:0.4f;
        m_button_group.interactable = isActive;        
    }
}
