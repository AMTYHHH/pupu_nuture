using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum BodyPartType{Head, Body, Hand, Foot}

namespace PupuMap
{
    //Pupu Map Manager和Alchemist Manager功能上有一定的相互依赖，可以考虑进一步将功能拆开，或是整合成一个完整的Manager
    public class PupuMapManager : Singleton<PupuMapManager>
    {
        [SerializeField] private MapSO mapSO;
    [Header("默认解锁地图")]
        [SerializeField] private string[] defaultUnlockMaps;
        public Dictionary<TattoData, string> BodyNameTattoDict{get; private set;} //该词典记录了Pupu的Tatto数据与对应的部位的名字。
        public string[] unlockMaps{get; private set;} //该词典记录了已解锁的地图
        protected override void Awake()
        {
            base.Awake();
        //To Do: 加载解锁的地图并赋值给unlockMaps
            unlockMaps = defaultUnlockMaps;
        //To Do:
        //1.加载玩家的Pupu自定义，并生成对应的BodyNameTattoDict
        //2.根据BodyNameTattoDict, 可以从本地文件夹中寻找是否存在对应的纹身贴图，如果没有，则可以根据TattoData里的数据重新创建对应的纹身并储存到本地。
            BodyNameTattoDict = new Dictionary<TattoData, string>();
        }
        public AssetReference GetMapRefrence(string mapName){return mapSO.GetMapReferenceByName(mapName);}
        public static void PrintTattoFromTexture(string tattoMapName, SpriteRenderer mapRenderer, TattoPart bodyPart, float downSampleLevel = 1f) //根据当前场景的地图坐标和Pupu身体坐标来裁剪纹理作为Pupu纹理
        {
            Transform mapTrans = mapRenderer.transform;
            Sprite mapSprite  = mapRenderer.sprite;
            
            Vector2 offset = mapTrans.position;

            Vector2 minOffset = bodyPart.bodyRenderer.bounds.min;
            Vector2 maxOffset = bodyPart.bodyRenderer.bounds.max;

        //To Do:此方法在地图缩小之后，会导致截取的贴图像素变大，因此需要对贴图采样进行一定的DownSample
            Vector2Int texSize = Vector2Int.CeilToInt((maxOffset-minOffset)*mapSprite.pixelsPerUnit/mapTrans.lossyScale.x); //此处默认贴图的x,y是1：1
            Vector2Int texOffset = Vector2Int.CeilToInt((minOffset-offset)*mapSprite.pixelsPerUnit/mapTrans.lossyScale.x); //此处默认贴图的x,y是1：1
            
            Texture2D temp = Texture_SnapShoter.TakeSnapshot(mapSprite.texture, bodyPart.gameObject.name, texOffset, texSize, downSampleLevel);

            bodyPart.PrintTatto(temp, mapRenderer.color);
            bodyPart.UpdateTattoData(tattoMapName, mapRenderer.color, texOffset, texSize);
        }
        public static void PrintTattoFromScreen(Camera mapCam, TattoPart bodyPart, float downSampleLevel = 1f) //根据当前场景摄像机和SpriteRender的信息截取屏幕截图作为Pupu纹理
        {
            Vector2 minOffset = mapCam.WorldToScreenPoint(bodyPart.bodyRenderer.bounds.min);
            Vector2 maxOffset = mapCam.WorldToScreenPoint(bodyPart.bodyRenderer.bounds.max);

            Vector2Int size = Vector2Int.RoundToInt(maxOffset-minOffset);

            minOffset.y = maxOffset.y;
            minOffset.y = Screen.height - minOffset.y;

            Texture2D temp = Screenshot_Taker.TakeScreenShot(mapCam, bodyPart.gameObject.name, Vector2Int.RoundToInt(minOffset), size);        

            bodyPart.PrintTatto(temp, Color.white); //由于银幕截图已经带有颜色，因此直接赋予白色
        }
    }
}