using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEditor.UI;
using UnityEngine;

namespace GameUI
{
    public class GuideDialog : Dialog
    {
        Controller pageState;

        protected override void OnInit()
        {
            base.OnInit();

            var bgBtn = this.contentPane.GetChild("bgBtn").asButton;
            Debug.Log("contentPaneName:" + contentPane.name);
            Debug.Log("bgBtnName:" + bgBtn.name);
            pageState = this.contentPane.GetController("page");

            bgBtn.onClick.Add(OnTriggerNextBtn);
        }

        protected override void OnShown()
        {
            base.OnShown();

        }

        private void OnTriggerNextBtn()
        {

            var pageNum = pageState.pageCount;
            var curPage = pageState.selectedIndex;
            if (curPage < pageNum - 1)
            {
                pageState.SetSelectedIndex(curPage + 1);
            }
            else
            {
                this.Hide();
                UIManager.OpenDialog(DialogType.MainDialog);
            }
        }

        private void OnTriggerCancel()
        {

        }
    }
}

