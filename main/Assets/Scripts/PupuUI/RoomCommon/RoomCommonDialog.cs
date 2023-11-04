using FairyGUI;
using GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class RoomCommonDialog : Dialog
    {
        private Controller isEdit;
        private GComponent editComp;

        protected override void OnInit()
        {
            base.OnInit();
            isEdit = this.contentPane.GetController("isEdit");
            editComp = this.contentPane.GetChild("editComp").asCom;
            var exitEditBtn = editComp.GetChild("closeBtn").asButton;
            exitEditBtn.onClick.Add(OnTriggerExitEdit);
        }

        public void EnterEditMode()
        {
            isEdit?.SetSelectedIndex(1);
        }

        public void ExitEditMode()
        {
            isEdit?.SetSelectedIndex(0);
        }

        private void SetEditState(bool isEditState)
        {
            isEdit?.SetSelectedIndex(isEditState ? 1 : 0);
        }

        private void OnTriggerExitEdit()
        {
            RoomManager.ChangeRoomEditState(false);
        }
    }
}
