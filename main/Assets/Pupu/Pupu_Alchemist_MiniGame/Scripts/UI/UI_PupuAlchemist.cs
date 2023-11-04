using System.Collections;
using System.Collections.Generic;
using AlchemistSystem;
using PupuMap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PupuAlchemist : MonoBehaviour
{
[Header("预览菜单")]
    [SerializeField] private GameObject previewCanvas;
[Header("开头菜单")]
    [SerializeField] private GameObject startMenuGameobject;
    [SerializeField] private UI_BodyPartButton[] bodyPartButtons;
    [SerializeField] private Button startAlchemistButton;
[Header("地图选择")]
    [SerializeField] private GameObject mapSelectionMenu;
    [SerializeField] private TextMeshProUGUI mapShowingName;
    [SerializeField] private RectTransform mapParentTrans;
    [SerializeField] private GameObject mapButtonPrefab;
[Header("地图缩放")]
    [SerializeField] private RectTransform pointerRect;
    [SerializeField] private Button zoomInButton;
    [SerializeField] private Button zoomOutButton;  
    [SerializeField, ShowOnly] private int ZoomLevel = 2; //缩放等级的范围为0~4，默认等级为2
[Header("玩家指示")]
    [SerializeField] private GameObject arrow;
[Header("碎片选择")]
    [SerializeField] private RectTransform fragmentButtonGroupTrans;
    [SerializeField] private GameObject fragmentButtonPrefab;
    [SerializeField] private CanvasGroup fragSelectCanvas;
[Header("颜色选择")]
    [SerializeField] private GameObject colorExpandPanel;
    [SerializeField] private UI_ColorButton[] colorButtons;
[Header("结算")]
    [SerializeField] private GameObject resultGroup;

    private int fragButtonAmount = 5; //当前设置下，碎片二级菜单里，最大生成的Fragment Button数量。可根据玩家获得的碎片种类和数量去调整，例如当前玩家每种最多只有2个碎片时，我们生成5个Fragment Button就足够
    private UI_ColorButton selectedColorButton;
    private UI_FragDirButton selectedFragTypeButton;
    private UI_BodyPartButton selectedBodyTypeButton;
    private UI_FragButton[] fragButtons;
    private UI_MapButton[] mapButtons;

#region Unity Event
    void OnEnable(){
        EventHandler.E_OnClickOnNonUI += OnClickOnNonUI_Handler;
        EventHandler.E_OnPlayerInViewChange += OnPlayerInViewChange_Handler;
    }
    void OnDisable(){
        EventHandler.E_OnClickOnNonUI -= OnClickOnNonUI_Handler;
        EventHandler.E_OnPlayerInViewChange -= OnPlayerInViewChange_Handler;
    }
    void Start(){
    //生成fragment button
        fragButtons = new UI_FragButton[fragButtonAmount];
        for(int i=0; i<fragButtonAmount; i++){
            var fragButton = GameObject.Instantiate(fragmentButtonPrefab);
            var rect = fragButton.GetComponent<RectTransform>();

            rect.SetParent(fragmentButtonGroupTrans); //按键的位置已在Parent中的Horizontal Layout Group 以及 Content Size Fitter里自动设置

            rect.gameObject.SetActive(false);
            fragButtons[i] = fragButton.GetComponent<UI_FragButton>();
        }
    //生成map button
        mapButtons = new UI_MapButton[PupuMapManager.Instance.unlockMaps.Length];
        for(int i=0; i<mapButtons.Length; i++){
            var mapButton = GameObject.Instantiate(mapButtonPrefab);
            var rect = mapButton.GetComponent<RectTransform>();

            rect.SetParent(mapParentTrans);

            mapButtons[i] = mapButton.GetComponent<UI_MapButton>();
            mapButtons[i].Initialize(PupuMapManager.Instance.unlockMaps[i]);
        }

        colorButtons[0].OnClick(); //游戏开始时选中第一个color作为默认颜色
        bodyPartButtons[0].OnClick();

        startMenuGameobject.SetActive(true);
    }
#endregion

#region Main Game
//点击“预览”或“退出”按钮时执行
    public void PreviewPupu()=>previewCanvas.gameObject.SetActive(true);
//实际退出界面时执行
    public void OnQuit(){}
//点击“返回修改”时执行
    public void OnReturn()=>previewCanvas.gameObject.SetActive(false);
#endregion

#region Button Event
    public void SelectFragmentDirection(FragmentDirection fragDir, UI_FragDirButton button){
        if(selectedFragTypeButton!=null) {
            selectedFragTypeButton.OnDeselect();
        //如果按下同一按键，则关闭菜单
            if(button == selectedFragTypeButton){
                selectedFragTypeButton = null;
                fragSelectCanvas.gameObject.SetActive(false);
                return;
            }
        }
        selectedFragTypeButton = button;
        selectedFragTypeButton.OnSelect();
        fragSelectCanvas.gameObject.SetActive(true);

    //根据选择的碎片方向，读取按键信息
        for(int i=0; i<fragButtons.Length; i++) fragButtons[i].gameObject.SetActive(false);

        List<FragmentCounter> fragInfos = Alchemist_Manager.Instance.fragmentDict[fragDir];
        for(int i=0; i<fragInfos.Count; i++){
            fragButtons[i].gameObject.SetActive(true);
            fragButtons[i].Initialize(fragInfos[i].fragNames, fragInfos[i].Amount);
        }
    }
    public void SwitchColorButton()=>colorExpandPanel.gameObject.SetActive(!colorExpandPanel.gameObject.activeSelf);
    public void SelectColor(ColorType colorType, UI_ColorButton colorButton){
    //清除之前button的选择，并选择新的button
        if(selectedColorButton!=null) selectedColorButton.OnDeselect();
        selectedColorButton = colorButton;
        selectedColorButton.OnSelect();
    //To Do：选择地图颜色
    }
    public void SelectBodyPart(BodyPartType bodyType, UI_BodyPartButton bodyButton){
        if(selectedBodyTypeButton!=null) selectedBodyTypeButton.OnDeselect();
        selectedBodyTypeButton = bodyButton;
        selectedBodyTypeButton.OnSelect();
    //To Do: 选择身体部分
    }
    public void EnterMapSelect(){
        mapSelectionMenu.gameObject.SetActive(true);
    }
    public void SelectMap(string mapName){
        mapSelectionMenu.gameObject.SetActive(false);
        mapShowingName.text = mapName;
        startAlchemistButton.interactable = true;
    }
    public void EnterAlchemistGame(){
        startMenuGameobject.gameObject.SetActive(false);
    }
    public void ConfirmResult() //点击结算页面的“确认”按钮时触发
    {
        resultGroup.SetActive(false);
    }
    public void ZoomInMap() //点击放大地图按钮时触发
    {
        if(ZoomLevel==0) zoomOutButton.interactable = true;

        ZoomLevel ++;
        pointerRect.anchoredPosition += Vector2.right*15;
        if(ZoomLevel>=4){
            ZoomLevel = 4;
            zoomInButton.interactable = false;
        } 
    }
    public void ZoomOutMap() //点击缩小地图按钮时触发
    {
        if(ZoomLevel==4) zoomInButton.interactable = true;

        ZoomLevel --;
        pointerRect.anchoredPosition -= Vector2.right*15;
        if(ZoomLevel<=0){
            ZoomLevel = 0;
            zoomOutButton.interactable = false;
        }
    }
    public void FindPlayer() //点击箭头时触发
    {
        EventHandler.Call_OnFindPlayer();
    }
#endregion
    void OnPlayerInViewChange_Handler(bool isInView){
        if(isInView) arrow.SetActive(false);
        else arrow.SetActive(true);
    }
    void OnClickOnNonUI_Handler(){
        if(selectedFragTypeButton!=null){
            selectedFragTypeButton.OnDeselect();
            selectedFragTypeButton = null;
        }
        fragSelectCanvas.gameObject.SetActive(false);
    }
}
