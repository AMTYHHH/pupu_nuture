using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using PupuMap;

public class TattoMap : MonoBehaviour
{
[Header("Select Body Part")]
    [SerializeField] private InputAction press;
[Header("Render Tatto Map")]
    [SerializeField] private Camera map_RT_Cam;
    [SerializeField] private SpriteRenderer[] tattoMapRenderers;
    private Camera mainCam;
    private TattoPart selectedBody;
    private RenderTexture tempTex;
    private AsyncOperationHandle<Sprite> assetHandle;
    private string currentMapName;
    private int TattoMapIndex = Shader.PropertyToID("TATTO_MAP_TEX");
    private LayerMask interactableLayer;

    void Awake(){
        interactableLayer = 1<<LayerMask.NameToLayer("Interactable");
        mainCam = Camera.main;
    }
    void OnEnable(){
        press.performed += SelectBody;
        press.Enable();

    //To Do:目前采用第二个camera来单独渲染地图，以做出选中部分高亮显示地图的效果，但应该有更好的办法，移动端需要减少render texture使用量
        tempTex = RenderTexture.GetTemporary(mainCam.pixelWidth, mainCam.pixelHeight, 0, RenderTextureFormat.Default);
        map_RT_Cam.enabled = true;
        map_RT_Cam.targetTexture = tempTex;

        Shader.SetGlobalTexture(TattoMapIndex, tempTex);
    }
    void OnDisable(){
        press.performed -= SelectBody;
        press.Disable();

        map_RT_Cam.enabled = false;
        tempTex.Release();

        Shader.SetGlobalTexture(TattoMapIndex, Texture2D.whiteTexture);
    }
    public void ConfirmCustomize(float scaleDownFactor){
        if(selectedBody==null){
            Debug.LogAssertion("No body part selected!!");
            return;
        }

    //根据当前map的位置计算出对应的texture的offset与size
        PupuMapManager.PrintTattoFromTexture(currentMapName, tattoMapRenderers[1], selectedBody, scaleDownFactor);
        selectedBody.OnDeselectBody();
        selectedBody = null;
    }
    public void UnloadMapFile(){
        for(int i=0; i<tattoMapRenderers.Length; i++){
            tattoMapRenderers[i].sprite = null;
        }

        if(assetHandle.Result != null) Addressables.Release(assetHandle);
        currentMapName = string.Empty;
    }
    public void LoadMapFile(string mapName){
        assetHandle = PupuMapManager.Instance.GetMapRefrence(mapName).LoadAssetAsync<Sprite>();

        assetHandle.Completed += (handle)=>
        {
            Sprite _sprite = handle.Result;
            for(int i=0; i<tattoMapRenderers.Length; i++){
                tattoMapRenderers[i].sprite = _sprite;
            }

            Transform mapTrans = tattoMapRenderers[0].transform;
            mapTrans.position = _sprite.texture.width/_sprite.pixelsPerUnit * 0.5f * Vector3.left * mapTrans.localScale.x +
                                _sprite.texture.height/_sprite.pixelsPerUnit * 0.5f * Vector3.down * mapTrans.localScale.y;
            
            currentMapName = mapName;
        };
    }
    void SelectBody(InputAction.CallbackContext callback){
        Vector3 dragPos = mainCam.ScreenToWorldPoint(Pointer.current.position.ReadValue());
        dragPos.z = 0;

        TattoPart bodyPart; 
        bodyPart = Physics2D.OverlapPoint(dragPos, interactableLayer)?.GetComponent<TattoPart>();
        if(bodyPart!=null){
            if(selectedBody!=null) selectedBody.OnDeselectBody();
            selectedBody = bodyPart;
            selectedBody.OnSelectBody();
        }
    }
}
