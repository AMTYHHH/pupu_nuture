using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

using FurnitureSystem;
using UnityEngine.Timeline;

public class FurnitureCustomize : MonoBehaviour
{
[Header("壁纸")]
    [SerializeField] private SpriteRenderer wallpaperSpriteRenderer;
[Header("地板")]
    [SerializeField] private SpriteRenderer floorSpriteRenderer;
[Header("各类家具")]
    [SerializeField] private Transform defaultHanging_pos;
[Space(10)]
    [SerializeField] private Transform wallmountedlayer;
    [SerializeField] private Transform furniturelayer_back;
    [SerializeField] private Transform furniturelayer_mid;
    [SerializeField] private Transform furniturelayer_front;

    [SerializeField] private InputAction press;
    [SerializeField] private InputAction move;
[Header("重新摆放家具")]
    [SerializeField] private float holdTime = 0.4f;
    [SerializeField] private bool selectionByLayer = false;

    private float holdTimer = 0;
    private Vector3 pointerPos;
    private Camera mainCam;
    private GameObject draggingFurniture; //正在被玩家拖拽的家具
    private GameObject selectedFurniture; //被玩家选中重新布置的家具

    private bool isOverUI = false;

    private FurnitureLayer activeLayer;

    void OnEnable(){
        Debug.Log("OnEnable");
        EventHandler.E_UI_OnSelectObject += OnSelectObjectHandler;
        EventHandler.E_UI_OnDragObject += OnDragObjectHandler;
        EventHandler.E_UI_OnSelectFurnitureLayer += OnSelectFLayerHandler;

        move.performed  += Drag;
        press.performed += TryPickUp;
        press.canceled  += TryPutDown;

        move.Enable();
        press.Enable();
    }
    void OnDisable()
    {
        Debug.Log("OnDisable");
        EventHandler.E_UI_OnSelectObject -= OnSelectObjectHandler;
        EventHandler.E_UI_OnDragObject -= OnDragObjectHandler;
        EventHandler.E_UI_OnSelectFurnitureLayer -= OnSelectFLayerHandler;

        move.performed  -= Drag;
        press.performed -= TryPickUp;
        press.canceled  -= TryPutDown;

        move.Disable();
        press.Disable();
    }
    void Start()
    {
        Debug.Log("Start");
        mainCam = Camera.main;

        pointerPos = new Vector3(0,0,0);

        string fName = FurnitureManager.Instance.PlacedWallpaper;
        if(fName == string.Empty) wallpaperSpriteRenderer.sprite = null;
        else wallpaperSpriteRenderer.sprite = FurnitureManager.Instance.GetWallpaperSprite(fName);

        fName = FurnitureManager.Instance.PlacedFloor;
        if(fName == string.Empty) floorSpriteRenderer.sprite = null;
        else floorSpriteRenderer.sprite = FurnitureManager.Instance.GetFloorSprite(fName);

        var placedFurnituresData = FurnitureManager.Instance.PlacedFurnituresData;
        if(placedFurnituresData.Count == 0 || placedFurnituresData == null) return;
        else{
            foreach(FurnitureData furnitureData in placedFurnituresData){
                GameObject fObject = SpawnFurniture(furnitureData.fName, 
                                                    furnitureData.fType, 
                                                    furnitureData.fLayer, 
                                                    furnitureData.position, 
                                                    furnitureData.rotation, true);
                                                    
                FurnitureManager.Instance.AddItemToFurnitureList(fObject.GetComponent<Furniture_Base>());
            }
        }
    }
    void Update(){

        //Debug.Log("Update");
        isOverUI = EventSystem.current.IsPointerOverGameObject();

        if(selectedFurniture!=null && draggingFurniture==null){
            holdTimer += Time.deltaTime;

            //检查是否持续按住所选的家具，若选择有变，则放弃拖动家具。
            Vector3 dragPos = mainCam.ScreenToWorldPoint(pointerPos);
            Debug.Log("UpdateDragPos-Start:" + dragPos.x + "," + dragPos.y + "," + dragPos.z + "");
            dragPos.z = 0;
            var fBase = detectFurnitureBaseOnWorldPos(dragPos);
            if(fBase==null || selectedFurniture!=fBase.gameObject){
                holdTimer = 0;
                selectedFurniture = null;
            }
        //如果超过检测时间，则开始拖动所选家具的副本
            if(holdTimer >= holdTime){
                draggingFurniture = SpawnFurniture(fBase.FName, fBase.FType, FurnitureManager.LayerMaskToFurnitureLayer(fBase.gameObject.layer),
                                                   dragPos, fBase.transform.rotation, false);
                draggingFurniture.GetComponent<Furniture_Base>().SwitchToDragging();
                draggingFurniture.name = fBase.gameObject.name; //由于是一个选中家具的副本，因此赋予相同的名字，方便做家具摆放合理性检测时，提前剔除掉它

                selectedFurniture.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
                selectedFurniture.GetComponent<Furniture_Base>().SwitchToStatic();

                Debug.Log("UpdateDragPos:" + dragPos.x + "," + dragPos.y + "," + dragPos.z + "");
            }
        }
    }
    //检测目标处是否有家具，并按照从前到后的顺序检测
    //To Do:也许此处应该有更优化的解决方案
    Furniture_Base detectFurnitureBaseOnWorldPos(Vector3 pointerWorldPos){
        Furniture_Base fBase = null; 
        if(selectionByLayer){
            fBase = Physics2D.OverlapPoint(pointerWorldPos, 1<<FurnitureManager.FurnitureLayerToLayerMask(activeLayer))?.GetComponent<Furniture_Base>();
        }
        else{
            fBase = Physics2D.OverlapPoint(pointerWorldPos, 1<<Service.Front_Layer)?.GetComponent<Furniture_Base>();
            if(fBase == null) fBase = Physics2D.OverlapPoint(pointerWorldPos, 1<<Service.Mid_Layer)?.GetComponent<Furniture_Base>();
            if(fBase == null) fBase = Physics2D.OverlapPoint(pointerWorldPos, 1<<Service.Back_Layer)?.GetComponent<Furniture_Base>();
            if(fBase == null) fBase = Physics2D.OverlapPoint(pointerWorldPos, 1<<Service.Default_Layer)?.GetComponent<Furniture_Base>();
        }

        return fBase;
    }
#region Input Event
    void TryPickUp(InputAction.CallbackContext callback){
        holdTimer = 0;
    //若在于UI交互，则返回
        if(isOverUI) return;

        Debug.Log("TryPickUp");
        //当没有与UI交互时，呼叫事件
        EventHandler.Call_OnClickOnNonUI();
    //尝试选定要移动位置的家具
        Vector3 dragPos = mainCam.ScreenToWorldPoint(pointerPos);
        dragPos.z = 0;

        var fBase = detectFurnitureBaseOnWorldPos(dragPos);
        if (fBase != null)
        {
            selectedFurniture = fBase.gameObject;
            Debug.Log("TryPickUp:" + selectedFurniture != null + " " + fBase.gameObject.name);
        }
    }
    void Drag(InputAction.CallbackContext callback)
    {
        pointerPos = callback.ReadValue<Vector2>();

        if(draggingFurniture == null) return;
        
        Debug.Log("Drag");
        Vector3 dragPos = mainCam.ScreenToWorldPoint(pointerPos);
        if(draggingFurniture.GetComponent<Furniture_Base>().FType == FurnitureType.HangingItem) dragPos.y = defaultHanging_pos.position.y;
        dragPos.z = 0;

        draggingFurniture.transform.position = dragPos;
    }
    void TryPutDown(InputAction.CallbackContext callback)
    {
        holdTimer = 0;
        if(draggingFurniture == null) return;

        Debug.Log("TryPutDown");
        Furniture_Base furniture = draggingFurniture.GetComponent<Furniture_Base>();
        
        if(furniture.IsOverlapping){
            Destroy(furniture.gameObject);

            if(selectedFurniture != null){
                selectedFurniture.GetComponent<SpriteRenderer>().color = Color.white;
                selectedFurniture.GetComponent<Furniture_Base>().SwitchToSimmode();
            }
        }
        else{
            FurnitureManager.Instance.AddItemToFurnitureList(furniture);
            furniture.SwitchToSimmode();
            
            if(selectedFurniture != null){
                FurnitureManager.Instance.RemoveItemFromFurnitureList(selectedFurniture.GetComponent<Furniture_Base>());
                Destroy(selectedFurniture);
            }
        }

        draggingFurniture = null;
    }
#endregion

#region Event Handler
    void OnDragObjectHandler(string fName, FurnitureType fType, FurnitureLayer fLayer){
        switch(fType){
            case FurnitureType.Wallpaper:
                wallpaperSpriteRenderer.sprite = FurnitureManager.Instance.GetWallpaperSprite(fName);
                FurnitureManager.Instance.PlacedWallpaper = fName;
                break;
            case FurnitureType.Floor:
                floorSpriteRenderer.sprite = FurnitureManager.Instance.GetFloorSprite(fName);
                FurnitureManager.Instance.PlacedFloor = fName;
                break;
            default:
                Vector3 dragPos = mainCam.ScreenToWorldPoint(pointerPos);
                if(fType == FurnitureType.HangingItem) dragPos.y = defaultHanging_pos.position.y;
                dragPos.z = 0;

                draggingFurniture = SpawnFurniture(fName, fType, fLayer, 
                                                dragPos,
                                                Quaternion.identity, false);

                draggingFurniture.GetComponent<Furniture_Base>().SwitchToDragging();
                break;
        }
    }
    void OnSelectFLayerHandler(FurnitureLayer fLayer)=>activeLayer = fLayer;
    void OnSelectObjectHandler(string fName, FurnitureType fType, FurnitureLayer fLayer, bool isTakeBack){
        switch(fType){
            case FurnitureType.Wallpaper:
                if(isTakeBack){
                    wallpaperSpriteRenderer.sprite = null;
                    FurnitureManager.Instance.PlacedWallpaper = string.Empty;
                }
                else{
                    wallpaperSpriteRenderer.sprite = FurnitureManager.Instance.GetWallpaperSprite(fName);
                    FurnitureManager.Instance.PlacedWallpaper = fName;
                }
                break;
            case FurnitureType.Floor:
                if(isTakeBack){
                    floorSpriteRenderer.sprite = null;
                    FurnitureManager.Instance.PlacedFloor = string.Empty;
                }
                else{
                    floorSpriteRenderer.sprite = FurnitureManager.Instance.GetFloorSprite(fName);
                    FurnitureManager.Instance.PlacedFloor = fName;
                }
                break;
            default:
                if(isTakeBack){
                    Furniture_Base fBase = FurnitureManager.Instance.GetFurnitureInScene(fName);
                    FurnitureManager.Instance.RemoveItemFromFurnitureList(fBase);
                    Destroy(fBase.gameObject);
                }
                else{
                    Vector3 spawnPos = defaultHanging_pos.position;
                    if(fType == FurnitureType.WallMountedItem) spawnPos += Vector3.down;
                    if(fType == FurnitureType.Furniture) spawnPos += Vector3.down*2;

                    GameObject fObject = SpawnFurniture(fName, fType, fLayer, spawnPos, Quaternion.identity, false);
                    if(fObject!=null){
                        FurnitureManager.Instance.AddItemToFurnitureList(fObject.GetComponent<Furniture_Base>());
                    }
                }
                break;
        }
    }
#endregion

    GameObject SpawnFurniture(string _fName, FurnitureType _fType, FurnitureLayer _fLayer, Vector3 position, Quaternion rotation, bool _isSleep){
        GameObject fObject = GameObject.Instantiate(FurnitureManager.Instance.GetItemGameObject(_fName, _fType));

        if(_fType == FurnitureType.WallMountedItem){
            fObject.transform.parent = wallmountedlayer;
            fObject.GetComponent<SpriteRenderer>().sortingLayerName = Service.Background_SLayer;
        }
        else{
            switch(_fLayer){
                case FurnitureLayer.Back:
                    fObject.transform.parent = furniturelayer_back;
                    fObject.layer = Service.Back_Layer;
                    fObject.GetComponent<SpriteRenderer>().sortingOrder = FurnitureManager.backOrder;
                    break;
                case FurnitureLayer.Mid:
                    fObject.transform.parent = furniturelayer_mid;
                    fObject.layer = Service.Mid_Layer;
                    fObject.GetComponent<SpriteRenderer>().sortingOrder = FurnitureManager.midOrder;
                    break;
                case FurnitureLayer.Front:
                    fObject.transform.parent = furniturelayer_front;
                    fObject.layer = Service.Front_Layer;
                    fObject.GetComponent<SpriteRenderer>().sortingOrder = FurnitureManager.frontOrder;
                    break;
            }
        }
    
        fObject.transform.position = position;
        fObject.transform.rotation = rotation;

        if(_isSleep) fObject.GetComponent<Furniture_Base>().SwitchToSleep();     
        fObject.name = _fName + $"_00{FurnitureManager.Instance.PlacedFurnitures.Count}";

        return fObject;
    }
}
