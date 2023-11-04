using FairyGUI;
using GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class UseGoodsDialog : Dialog
    {
        private GLoader icon;
        private GTextField name;
        private GTextField desc;
        private GTextField num;

        protected override void OnInit()
        {
            base.OnInit();
            icon = contentPane.GetChild("icon").asLoader;
            name = contentPane.GetChild("name").asTextField;
            desc = contentPane.GetChild("desc").asTextField;
            num = contentPane.GetChild("num").asTextField;

            var closeBtn = contentPane.GetChild("closeBtn").asButton;
            closeBtn.onClick.Add(() => Hide());
            
            var cancelBtn = contentPane.GetChild("cancelBtn").asButton;
            cancelBtn.onClick.Add(() => Hide());
        }

        public void SetGoodsInfo(GoodsInfoData goods)
        {
            Debug.Log("showGoodsInfo");
            if(goods == null)
            {
                Hide();
                return;
            }
            //icon.url = goods.icon;
            name.text = goods.name;
            desc.text = goods.desc;
            num.text = "x" + goods.num.ToString();
        }
    }
}
