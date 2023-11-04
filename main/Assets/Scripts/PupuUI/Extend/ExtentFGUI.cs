using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

namespace GameUI
{
    public static class ExtentFariyGUI
    {
        public static void SetContent(this GTextField gTextField, string content)
        {
            gTextField.text = content;
        }
    }

    //public static void GTextField::SetContent(this, string content)
    //{

    //}
}

