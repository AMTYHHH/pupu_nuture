using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum FurnitureType{Wallpaper, Floor, Furniture, SmallOrnaments, WallMountedItem, HangingItem}
public enum FurnitureLayer{Back, Mid, Front, None}
public enum FurnitureState{Dragging, Simulating}

namespace FurnitureSystem{
    public class FurnitureManager:Singleton<FurnitureManager>
    {
        [SerializeField] private FurnitureList_SO object_list_SO;
        public int MaxFurniture;
    [Header("Default Unlocked")] //To Do:开始游戏前，载入已解锁的家具文件，并赋值给对应的unlock list
        public List<Unlock_List> unlock_Lists;
    [Header("Furniture Placement")] //To Do:开始游戏前，载入已放置的家具文件，并赋值给对应的家具数据，并恢复数据
        [HideInInspector] public string PlacedWallpaper;
        [HideInInspector] public string PlacedFloor;

        public bool IsReachMaxFurniture{get{return PlacedFurnitures.Count >= MaxFurniture;}}
        public List<Furniture_Base> PlacedFurnitures{get; private set;} //用于实时记录场景中家具物件的列表，并在储存布局时，将数据转换为FurnitureData
        public List<FurnitureData> PlacedFurnituresData{get; private set;} //用于储存和读取的家具数据列表，读取时加载该数据

        public const int backOrder = -5;
        public const int midOrder = 0;
        public const int frontOrder = 5;

        protected override void Awake()
        {
            base.Awake();

        //To Do:加载玩家的家具布局，并生成对应的FurnitureData与Furnitures
            PlacedFurnituresData = new List<FurnitureData>();
            PlacedFurnitures = new List<Furniture_Base>();
        }
        void UpdatePlacedFurnituresData() //用于将场景中摆放的家具转换为家具数据，用于保存
        {
            PlacedFurnituresData.Clear();

            for(int i=0; i<PlacedFurnitures.Count; i++){
                var data = new FurnitureData(){
                    position = PlacedFurnitures[i].transform.position,
                    rotation = PlacedFurnitures[i].transform.rotation,
                    fType  = PlacedFurnitures[i].FType,
                    fLayer = LayerMaskToFurnitureLayer(PlacedFurnitures[i].gameObject.layer),
                    fName  = PlacedFurnitures[i].FName
                };

                PlacedFurnituresData.Add(data);
            } 
        }

        public static FurnitureLayer LayerMaskToFurnitureLayer(LayerMask layerMask)//将Unity LayerMask转换为对应的FurnitureLayer
        {
            if(layerMask == Service.Back_Layer) return FurnitureLayer.Back;
            if(layerMask == Service.Mid_Layer) return FurnitureLayer.Mid;
            if(layerMask == Service.Front_Layer) return FurnitureLayer.Front;

            return FurnitureLayer.None;
        }
        public static LayerMask FurnitureLayerToLayerMask(FurnitureLayer fLayer)//将FurnitureLayer转换为对应的Unity LayerMask
        {
            switch(fLayer){
                case FurnitureLayer.Front:
                    return Service.Front_Layer;
                case FurnitureLayer.Mid:
                    return Service.Mid_Layer;
                case FurnitureLayer.Back:
                    return Service.Back_Layer;
                default:
                    return Service.Default_Layer;
            }
        }

        public List<string> GetUnlockList(FurnitureType fType){return unlock_Lists.Find(x=>x.furnitureType == fType).Unlock_list;}
        public string GetMenuName(string fName, FurnitureType fType){return object_list_SO.GetMenuName(fName, fType);}
        public GameObject GetItemGameObject(string fName, FurnitureType fType){return object_list_SO.GetItemGameObject(fName, fType);}
        public Furniture_Base GetFurnitureInScene(string fName){return PlacedFurnitures.Find(x=>x.FName == fName);}
        public Sprite GetWallpaperSprite(string fName){return object_list_SO.GetWallpaperSprite(fName);}
        public Sprite GetFloorSprite(string fName){return object_list_SO.GetFloorSprite(fName);}
        public Sprite GetMenuSprite(string fName, FurnitureType fType){return object_list_SO.GetMenuSprite(fName, fType);}

        public void AddItemToFurnitureList(Furniture_Base furniture){
            if(PlacedFurnitures.Contains(furniture)){
                Debug.LogError($"Error:Already have {furniture.gameObject.name} in the list!!!");
                return;
            } 
            PlacedFurnitures.Add(furniture);

            EventHandler.Call_OnPlacedFurniture_Changed(furniture);
        }
        public void RemoveItemFromFurnitureList(Furniture_Base furniture){
            if(!PlacedFurnitures.Contains(furniture)){
                Debug.LogError($"Error:Can't find {furniture.gameObject.name} in the List!!!");
                return;
            }
            PlacedFurnitures.Remove(furniture);

            EventHandler.Call_OnPlacedFurniture_Changed(furniture);
        }
        public bool HasFurnitureInScene(string _fName, FurnitureType _fType){
            switch(_fType){
                case FurnitureType.Wallpaper:
                    return _fName == PlacedWallpaper;
                case FurnitureType.Floor:
                    return _fName == PlacedFloor;
                default:
                    var fObj = PlacedFurnitures.Find(x=>x.FName == _fName);
                    return fObj!=null;
            }
        }
    }

    [System.Serializable]
    public class Unlock_List{
        public FurnitureType furnitureType;
        public List<string> Unlock_list;
    }
    [System.Serializable]
    public class FurnitureData{
        public Vector3 position;
        public quaternion rotation;

        public FurnitureType fType;
        public FurnitureLayer fLayer;
        public string fName;
    }
}
