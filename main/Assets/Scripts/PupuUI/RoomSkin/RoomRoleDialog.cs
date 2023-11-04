using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class RoomRoleDialog : Dialog
    {

        private List<SkinInfoData> skinDataList;

        protected override void OnInit()
        {
            base.OnInit();


            skinDataList = new List<SkinInfoData>();
            skinDataList.Add(new SkinInfoData { type = SkinType.Food, icon = "ui://RoomLobby/type_1", name = "苹果", num = 1, desc = "很好" });
            skinDataList.Add(new SkinInfoData { type = SkinType.Food, icon = "ui://RoomLobby/type_2", name = "冰淇凌", num = 1, desc = "一般" });
            skinDataList.Add(new SkinInfoData { type = SkinType.Play, icon = "ui://RoomLobby/type_3", name = "篮球", num = 1, desc = "很好" });

        }


        // 道具种类
        public enum SkinType
        {
            Food = 1,       // 事物
            Play = 2,       // 玩具
            Study = 3,      // 学习
        }
        public class SkinInfoData
        {
            public SkinType type;
            public string icon;
            public string name;
            public string desc;
            public int num;
        }
    }
}

