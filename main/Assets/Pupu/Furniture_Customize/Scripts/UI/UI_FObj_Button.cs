using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using FurnitureSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_FObj_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TextMeshProUGUI object_Label;
    [SerializeField] private FurnitureType fType;
    [SerializeField] private string fName;
    [SerializeField] private float holdTime;

    private Image m_image;
    private Button m_button;
    private bool holding;
    private float timer;

    private static UI_FurnitureCustomize ui_fCustomize;
    void Awake(){
        if(ui_fCustomize==null){ui_fCustomize = FindObjectOfType<UI_FurnitureCustomize>();}

        m_image = GetComponent<Image>();
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnClick);
    }
    void OnDestroy(){
        m_button.onClick.RemoveListener(OnClick);
    }
    void Update(){
        if(holding){
            timer += Time.deltaTime;
            if(timer >= holdTime){
                holding = false;
                ui_fCustomize.DragFurnitureObject(fName, fType, this);
                
                timer = 0;
            }
        }
    }
    public void Initialize(FurnitureType _fType, string _fName, string _menuName, Sprite _menuSprite){
        holding = false;
        timer = 0;
        
        object_Label.text = _menuName;
        fName = _fName;
        fType = _fType;

        UpdateButtonColor();
    }
    public void OnClick(){
        ui_fCustomize.SelectFurnitureObject(fName, fType, this);
        UpdateButtonColor();
    }
    public void OnPointerDown(PointerEventData eventData){
        if(FurnitureManager.Instance.HasFurnitureInScene(fName, fType)) return;

        holding = true;
    }
    public void OnPointerUp(PointerEventData eventData){
        holding = false;
        timer = 0;
    }
    public void UpdateButtonColor()=>ChangeButtonColor(FurnitureManager.Instance.HasFurnitureInScene(fName, fType));
    void ChangeButtonColor(bool isPlaced){
        if(isPlaced) m_image.color = Color.white;
        else m_image.color = Color.magenta;
    }
}
