using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pupu/FragmentInfo_SO")]
public class FragmentInfo_SO : ScriptableObject
{
    [SerializeField] private List<FragmentInfo> fragmentInfos;
    public FragmentInfo GetFragmentInfoByName(string fragName){return fragmentInfos.Find(x=>x.fragmentName == fragName);}
}

[System.Serializable]
public class FragmentInfo{
    public string fragmentName;
    public Sprite fragmentIcon; //后续可以更换成Addressable，动态加载
    public FragmentDirection fragmentDirection;
    public FragmentRoute fragmentRoute;
}
