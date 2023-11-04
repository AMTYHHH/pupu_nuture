using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Pupu/Map_SO")]
public class MapSO : ScriptableObject
{
    [SerializeField] private List<MapData> mapDatas;
    public AssetReference GetMapReferenceByName(string name){return mapDatas.Find(x=>x.MapName == name).MapRef;}
}

[System.Serializable]
public class MapData{
    public string MapName;
    public AssetReference MapRef;
}
