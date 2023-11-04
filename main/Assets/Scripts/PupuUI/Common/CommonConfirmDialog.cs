using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace GameUI
{
    public class CommonConfirmDialog : Dialog
    {
        private EventCallback0 okCallBack;
        private EventCallback0 cancelCallBack;
        //public CommonConfirmDialog()
        //{

        //}
        //public CommonConfirmDialog(EventCallback0 okCallbackTem, EventCallback0 cancelCallbackTem = null)
        //{
        //    okCallBack = okCallbackTem;
        //    cancelCallBack = cancelCallbackTem;
        //}

        public void SetEventCallback(EventCallback0 okCallbackTem = null, EventCallback0 cancelCallbackTem = null)
        {
            okCallBack = okCallbackTem;
            cancelCallBack = cancelCallbackTem;
        }

        public void SetConfirmContent(string content)
        {
            var title = this.contentPane.GetChild("content").asTextField;
            title?.SetContent(content);
        }

        protected override void OnInit()
        {
            base.OnInit();
            base.OnInit();

            var ok = this.contentPane.GetChild("ok");
            ok.onClick.Set(OnTriggerOk);
            var cancel = this.contentPane.GetChild("cancel");
            cancel.onClick.Set(OnTriggerCancel);
        }

        protected override void OnShown()
        {
            base.OnShown();
        }

        private void OnTriggerOk()
        {
            Debug.Log("OnTriggerOk");
            if (okCallBack != null)
                okCallBack();
            else
                this.Hide();
        }

        private void OnTriggerCancel()
        {
            Debug.Log("OnTriggerCancel");
            if (cancelCallBack != null)
                cancelCallBack();
            else
                this.Hide();
        }
    }
}

