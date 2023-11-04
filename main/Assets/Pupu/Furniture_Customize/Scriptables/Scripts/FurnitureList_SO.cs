using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pupu/FurnitureList_SO")]
public class FurnitureList_SO : ScriptableObject
{
[Header("Background")]
    [SerializeField] private List<WallpaperInfo> wallpaperInfos;
    [SerializeField] private List<FloorInfo> floorInfos;
[Header("Item")]
    [SerializeField] private List<ItemInfo> furnitureInfos;
    [SerializeField] private List<ItemInfo> hangingItemInfos;
    [SerializeField] private List<ItemInfo> wallmountedInfos;
    [SerializeField] private List<ItemInfo> smallOrnamentInfos;

    public GameObject GetItemGameObject(string fName, FurnitureType fType){
        GameObject _fObject = null;
        switch(fType){
            case FurnitureType.Furniture:
                _fObject = furnitureInfos.Find(x=>x.FurnitureName == fName).FurnitureObject;
                break;
            case FurnitureType.SmallOrnaments:
                _fObject = smallOrnamentInfos.Find(x=>x.FurnitureName == fName).FurnitureObject;
                break;
            case FurnitureType.WallMountedItem:
                _fObject = wallmountedInfos.Find(x=>x.FurnitureName == fName).FurnitureObject;
                break;
            case FurnitureType.HangingItem:
                _fObject = hangingItemInfos.Find(x=>x.FurnitureName == fName).FurnitureObject;
                break;
        }

        return _fObject;
    }
    public Sprite GetFloorSprite(string fName){return floorInfos.Find(x=>x.FloorName == fName).FloorSprite;}
    public Sprite GetWallpaperSprite(string fName){return wallpaperInfos.Find(x=>x.WallpaperName == fName).WallpaperSprite;}
    public Sprite GetMenuSprite(string fName, FurnitureType fType){
        Sprite _sprite = null;
        switch(fType){
            case FurnitureType.Floor:
                _sprite = floorInfos.Find(x=>x.FloorName == fName).MenuIcon;
                break;
            case FurnitureType.Wallpaper:
                _sprite = wallpaperInfos.Find(x=>x.WallpaperName == fName).MenuIcon;
                break;
            case FurnitureType.Furniture:
                _sprite = furnitureInfos.Find(x=>x.FurnitureName == fName).MenuIcon;
                break;
            case FurnitureType.SmallOrnaments:
                _sprite = smallOrnamentInfos.Find(x=>x.FurnitureName == fName).MenuIcon;
                break;
            case FurnitureType.WallMountedItem:
                _sprite = wallmountedInfos.Find(x=>x.FurnitureName == fName).MenuIcon;
                break;
            case FurnitureType.HangingItem:
                _sprite = hangingItemInfos.Find(x=>x.FurnitureName == fName).MenuIcon;
                break;
        }

        return _sprite;
    }
    public string GetMenuName(string fName, FurnitureType fType){
        string _name = null;
        switch(fType){
            case FurnitureType.Floor:
                _name = floorInfos.Find(x=>x.FloorName == fName).MenuName;
                break;
            case FurnitureType.Wallpaper:
                _name = wallpaperInfos.Find(x=>x.WallpaperName == fName).MenuName;
                break;
            case FurnitureType.Furniture:
                _name = furnitureInfos.Find(x=>x.FurnitureName == fName).MenuName;
                break;
            case FurnitureType.SmallOrnaments:
                _name = smallOrnamentInfos.Find(x=>x.FurnitureName == fName).MenuName;
                break;
            case FurnitureType.WallMountedItem:
                _name = wallmountedInfos.Find(x=>x.FurnitureName == fName).MenuName;
                break;
            case FurnitureType.HangingItem:
                _name = hangingItemInfos.Find(x=>x.FurnitureName == fName).MenuName;
                break;
        }

        return _name;
    }
}

[System.Serializable] //壁纸
public class WallpaperInfo{
    public string WallpaperName;
    public Sprite WallpaperSprite;

    public string MenuName; //菜单中显示的名称
    public Sprite MenuIcon; //菜单中显示的缩略图
}

[System.Serializable] //地板
public class FloorInfo{
    public string FloorName;
    public Sprite FloorSprite;

    public string MenuName; //菜单中显示的名称
    public Sprite MenuIcon; //菜单中显示的缩略图
}

[System.Serializable] //可放置物品
public class ItemInfo{
    public string FurnitureName;
    public GameObject FurnitureObject;

    public string MenuName; //菜单中显示的名称
    public Sprite MenuIcon; //菜单中显示的缩略图
}
