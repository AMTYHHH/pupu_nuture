using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FurnitureSystem;
using TMPro;

public class UI_FurnitureCustomize : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI maxFurniture_text;

    [SerializeField] private CanvasGroup objectSelection_Canvas;
    [SerializeField] private RectTransform objectSelection_Content;
    [SerializeField] private GameObject objectSelectButtonPrefab;

    [SerializeField] private UI_FLayer_Button[] layerButtons;

    private UI_FObj_Button[] objectSelectionButtonsPool;
    private int objectButtonAmount = 30;

    [SerializeField, ShowOnly] private FurnitureLayer selected_fLayer;

    [SerializeField, ShowOnly] private UI_FObj_Button selectedFloorButton;
    [SerializeField, ShowOnly] private UI_FObj_Button selectedWallpaperButton;

    private UI_FLayer_Button selectedLayerButton;

    void OnEnable(){
        objectSelection_Canvas.gameObject.SetActive(false);

        objectSelectionButtonsPool = new UI_FObj_Button[objectButtonAmount];
        for(int i=0; i<objectButtonAmount; i++){
            var selectionButton = GameObject.Instantiate(objectSelectButtonPrefab);
            var rect = selectionButton.GetComponent<RectTransform>();

            rect.SetParent(objectSelection_Content);
            rect.localScale = Vector3.one;
            rect.anchoredPosition = new Vector3(i%4 * 170, -i/4 * 170);
            Vector3 localPos = rect.localPosition;
            localPos.z = 0;
            rect.localPosition = localPos;

            selectionButton.gameObject.SetActive(false);

            objectSelectionButtonsPool[i] = selectionButton.GetComponent<UI_FObj_Button>();
        }

        EventHandler.E_OnPlacedFurniture_Changed += OnPlacedFurnitureChanged_Handler;
        EventHandler.E_OnClickOnNonUI += OnClickOnNonUI_Handler;
    }
    void OnDisable(){
        EventHandler.E_OnPlacedFurniture_Changed -= OnPlacedFurnitureChanged_Handler;
        EventHandler.E_OnClickOnNonUI -= OnClickOnNonUI_Handler;
    }
    void Start(){
        maxFurniture_text.text = $"{FurnitureManager.Instance.PlacedFurnitures.Count}/{FurnitureManager.Instance.MaxFurniture}";
    //提前选择最后一个layer作为默认layer
        layerButtons[2].OnClick();
    //关掉所有layer button
        SwitchAllLayerButtons(true);
    }
    
#region Event Handler
    void OnPlacedFurnitureChanged_Handler(Furniture_Base furniture){
        maxFurniture_text.text = $"{FurnitureManager.Instance.PlacedFurnitures.Count}/{FurnitureManager.Instance.MaxFurniture}";
    }
    void OnClickOnNonUI_Handler(){
        CloseSelectionCanvas();
    }
#endregion

    void SwitchAllLayerButtons(bool isOn){
        for(int i=0; i<layerButtons.Length; i++){
            layerButtons[i].SwitchButton(isOn);
        }
    }
    bool CheckFurnitureSpawnedCondition(FurnitureType _fType){
        if(_fType!=FurnitureType.Wallpaper && _fType!=FurnitureType.Floor){
            if(FurnitureManager.Instance.IsReachMaxFurniture) {
                Debug.LogWarning("超过家具数量上限");
                return false;
            }
        }
        return true;
    }

#region Unity Button Function
//点击“家具种类”按钮时执行
    public void OnSelectFurnitureType(FurnitureType fType){
        Debug.Log($"Select {fType}");

        List<string> unlockList = FurnitureManager.Instance.GetUnlockList(fType);
        
        string menuName;
        Sprite menuSprite;

        for(int i=0; i<objectSelectionButtonsPool.Length; i++) objectSelectionButtonsPool[i].gameObject.SetActive(false);
        
        for(int i=0; i<unlockList.Count; i++){
            menuName = FurnitureManager.Instance.GetMenuName(unlockList[i], fType);
            menuSprite = FurnitureManager.Instance.GetMenuSprite(unlockList[i], fType);

            objectSelectionButtonsPool[i].Initialize(fType, unlockList[i],menuName, menuSprite);
            objectSelectionButtonsPool[i].gameObject.SetActive(true);
        }
        
        if(fType==FurnitureType.Wallpaper || fType == FurnitureType.Floor){
            maxFurniture_text.enabled = false;
            SwitchAllLayerButtons(false);
        }
        else{
            maxFurniture_text.enabled = true;
            if(fType == FurnitureType.WallMountedItem) SwitchAllLayerButtons(false);
            else SwitchAllLayerButtons(true);
        }

        objectSelection_Canvas.gameObject.SetActive(true);
    }
//点击某个具体家具按钮时执行
    public void SelectFurnitureObject(string _fName, FurnitureType _fType, UI_FObj_Button fObj_button){
        bool hasFurniture = FurnitureManager.Instance.HasFurnitureInScene(_fName, _fType);
        if(hasFurniture){
            EventHandler.Call_UI_OnSelectObject(_fName, _fType, selected_fLayer, true);
        }
        else{
            if(CheckFurnitureSpawnedCondition(_fType)){
                EventHandler.Call_UI_OnSelectObject(_fName, _fType, selected_fLayer, false);

                switch(_fType){
                    case FurnitureType.Floor:
                        if(selectedFloorButton!=null) selectedFloorButton.UpdateButtonColor();
                        selectedFloorButton = fObj_button;
                        break;
                    case FurnitureType.Wallpaper:
                        if(selectedWallpaperButton!=null) selectedWallpaperButton.UpdateButtonColor();
                        selectedWallpaperButton = fObj_button;
                        break;
                }
            }
        }
    }
//点击并拖动某个具体家具按钮时执行
    public void DragFurnitureObject(string _fName, FurnitureType _fType, UI_FObj_Button fObj_Button){
        if(CheckFurnitureSpawnedCondition(_fType)){
            EventHandler.Call_UI_OnDragObject(_fName, _fType, selected_fLayer);
            CloseSelectionCanvas();
        }
    }
    public void SelectFurnitureLayer(FurnitureLayer _fLayer, UI_FLayer_Button _fButton){
        selected_fLayer = _fLayer;
        EventHandler.Call_UI_OnSelectFurnitureLayer(_fLayer);

        if(selectedLayerButton!=null) selectedLayerButton.Deselected();
        selectedLayerButton = _fButton;
        selectedLayerButton.Selected();
    }
    public void CloseSelectionCanvas(){
        objectSelection_Canvas.gameObject.SetActive(false);
        SwitchAllLayerButtons(true);
    }
#endregion
}