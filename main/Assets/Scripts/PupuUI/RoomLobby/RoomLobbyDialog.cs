using FairyGUI;
using System.Collections.Generic;

namespace GameUI
{
    public class RoomLobbyDialog : Dialog
    {
        private GList goodsTypeList;
        private GList goodsList;

        private Controller showDetailList;

        private List<GoodsInfoData> goodsDataList;

        protected override void OnInit()
        {
            base.OnInit();

            goodsTypeList = contentPane.GetChild("goodsTypeList").asList;
            goodsList = contentPane.GetChildByPath("detailCom.goodsList").asList;

            showDetailList = contentPane.GetController("showDetailList");
            var closeDetailBtn = contentPane.GetChildByPath("detailCom.closeBtn").asButton;
            closeDetailBtn.onClick.Set(OnTriggerExitDetail);

            var hideDetailBtn = contentPane.GetChildByPath("hideDetailBtn").asButton;
            hideDetailBtn.onClick.Set(OnTriggerExitDetail);

            goodsDataList = new List<GoodsInfoData>();
            goodsDataList.Add(new GoodsInfoData { type = GoodsType.Food, icon = "ui://RoomLobby/type_1", name = "苹果", num = 1, desc = "很好" });
            goodsDataList.Add(new GoodsInfoData { type = GoodsType.Food, icon = "ui://RoomLobby/type_2", name = "冰淇凌", num = 1, desc = "一般" });
            goodsDataList.Add(new GoodsInfoData { type = GoodsType.Play, icon = "ui://RoomLobby/type_3", name = "篮球", num = 1, desc = "很好" });

            goodsTypeList.onClickItem.Add(OnSelectTypeList);
            goodsList.onClickItem.Add(OnSelectGoodsList);
        }

        protected override void OnShown()
        {
            base.OnShown();
            UpdateList();
        }

        private void UpdateList()
        {
            //goodsTypeList.RemoveChildrenToPool();
            for (var i = 0; i < goodsTypeList.numChildren; i++)
            {
                var item = goodsTypeList.GetChildAt(i).asCom;
                item.data = (GoodsType)i + 1;
            }
        }

        private void OnSelectTypeList(EventContext context)
        {
            goodsList.RemoveChildrenToPool();
            var selItem = (GObject)context.data;
            var type = (GoodsType)selItem.data;
            foreach (var goodsData in goodsDataList)
            {
                if(goodsData.type == type)
                {
                    var item = goodsList.AddItemFromPool().asCom;
                    item.GetChild("icon").asLoader.url = goodsData.icon;
                    item.GetChild("title").asTextField.text = goodsData.name;
                    item.data = goodsData;
                }
            }

            showDetailList.selectedIndex = 1;
            //_input1.ReplaceSelection("[:" + item.text + "]");
        }
        
        private void OnSelectGoodsList(EventContext context)
        {
            var selItem = (GObject)context.data;
            UIManager.OpenDialog(DialogType.UseGoodsDialog);

            var useGoodsDialog = UIManager.GetDialog("UseGoodsDialog") as UseGoodsDialog;
            useGoodsDialog.SetGoodsInfo(selItem.data as GoodsInfoData);
        }

        private void OnTriggerExitDetail()
        {
            showDetailList.selectedIndex = 0;
            goodsTypeList.ClearSelection();
        }
    }

    // 道具种类
    public enum GoodsType
    {
        Food = 1,       // 事物
        Play = 2,       // 玩具
        Study = 3,      // 学习
    }
    public class GoodsInfoData
    {
        public GoodsType type;
        public string icon;
        public string name;
        public string desc;
        public int num;
    }
}
