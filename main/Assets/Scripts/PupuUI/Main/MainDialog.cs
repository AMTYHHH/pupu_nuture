using FairyGUI;
using GameUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameUI
{
    public class MainDialog : Dialog
    {
        private GButton settingBtn;
        private GButton guideBtn;
        private GButton infoBtn = null;
        private GButton roomBtn;
        private GButton leftBtn;
        private GButton rightBtn;
        private GTextField roomName;

        private GComponent content;

        protected override void OnInit()
        {
            base.OnInit();

            settingBtn = contentPane.GetChild("settingBtn").asButton;
            guideBtn = contentPane.GetChild("guideBtn").asButton;
            infoBtn = contentPane.GetChild("infoBtn").asButton;
            roomBtn = contentPane.GetChild("roomBtn").asButton;
            leftBtn = contentPane.GetChild("leftBtn").asButton;
            rightBtn = contentPane.GetChild("rightBtn").asButton;

            roomName = contentPane.GetChild("roomName").asTextField;

            //Type t = typeof(MainDialog);
            //Debug.Log("  typeName:" + t.Name);
            //var childList = this.contentPane.GetChildren();
            //var list = t.GetProperties();
            //foreach (var item in list)
            //{
            //    Debug.Log("PropertyTypeName:" + item.Name + " className:" + item.GetType().Name);
            //}

            settingBtn.onClick.Add(OnTriggerSettingBtn);
            guideBtn.onClick.Add(OnTriggerGuideBtn);
            infoBtn.onClick.Add(OnTriggerInfoBtn);
            roomBtn.onClick.Add(OnTriggerRoomBtn);
            leftBtn.onClick.Add(()=>ChangeRoomIndex(-1));
            rightBtn.onClick.Add(() => ChangeRoomIndex(1));

            RoomManager.OpenRommDialog();
            UpdateRoom();
        }

        protected override void OnShown()
        {
            base.OnShown();
        }

        private void UpdateRoom()
        {
            var roomInfoData = RoomManager.GetCurRoomInfoData();
            roomName.text = roomInfoData.roomName;
        }

        private void ChangeRoomIndex(int roomChangeIndex)
        {
            RoomManager.ChangeRoom(roomChangeIndex);
            UpdateRoom();
        }

        private void OnTriggerSettingBtn()
        {
            //Debug.Log("OnTriggerSettingBtn");
        }
        private void OnTriggerGuideBtn()
        {
            //Debug.Log("OnTriggerGuideBtn");
        }
        private void OnTriggerInfoBtn()
        {
            //Debug.Log("OnTriggerInfoBtn");
        }
        private void OnTriggerRoomBtn()
        {
            //Debug.Log("OnTriggerRoomBtn");
            RoomManager.ChangeRoomEditState(true);
        }
    }

}

