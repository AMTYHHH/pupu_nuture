using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FragmentDirection{Left, Right, Up, Down}
public enum FragmentRoute{Stright, C_Shape, Other}

namespace AlchemistSystem{
    public class Alchemist_Manager : Singleton<Alchemist_Manager>
    {
        [SerializeField] private ColorPalette_SO colorPalette;
        [SerializeField] private FragmentInfo_SO fragmentInfo;
    [Header("默认已有的碎片")] //To Do: 此为测试用的设置，实际应该在开始游戏前或进入场景前，读取玩家的inventory信息，并赋值给fragmentDict
        [SerializeField] private FragmentCounter[] defaultFragmentsInHand;

        public Dictionary<FragmentDirection, List<FragmentCounter>> fragmentDict{get; private set;} //用于实时记录

        protected override void Awake(){
            base.Awake();

        //To Do：加载玩家的Inventory，并赋值给fragmentDict，这里用测试数据代替
            fragmentDict = new Dictionary<FragmentDirection, List<FragmentCounter>>();
            for(int i=0; i<defaultFragmentsInHand.Length; i++){
                var fragInfo = fragmentInfo.GetFragmentInfoByName(defaultFragmentsInHand[i].fragNames);
                if(!fragmentDict.ContainsKey(fragInfo.fragmentDirection)){
                    fragmentDict[fragInfo.fragmentDirection] = new List<FragmentCounter>();
                }
                fragmentDict[fragInfo.fragmentDirection].Add(defaultFragmentsInHand[i]);
            }
        }
        public Color GetColorFromPalette(ColorType colorType){return colorPalette.GetColorByName(colorType);}
    }
    [System.Serializable]
    public class FragmentCounter{
        public string fragNames;
        public int Amount;
    }
}