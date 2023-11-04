using FairyGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UI;
using UnityEngine;

namespace GameUI
{
    public class Dialog : Window
    {
        public bool isMask;

        protected override void OnInit()
        {
            base.OnInit();

            if (isMask)
            {
                
            }
            //Type t = this.GetType();
            //Debug.Log("  typeName:" + t.Name);
            //var childList = this.contentPane.GetChildren();
            //var list = t.GetProperties(BindingFlags.DeclaredOnly);
            //foreach(var item in list)
            //{
            //    Debug.Log("PropertyTypeName:" + item.Name + " className:" + item.GetType().Name);
            //}
            //foreach (var child in childList)
            //{
            //    Debug.Log("init:" + child.name);
            //    var attribute = t.GetProperty(child.name, typeof(object), new Type[] { typeof(GButton) });
            //    if (attribute != null)
            //    {
            //        Debug.Log("attributeType:" + attribute.GetType().Name);
            //        if(attribute.GetType() == typeof(GButton))
            //            attribute.SetValue(this, child as GButton);
            //    }
            //}
            //foreach(PropertyInfo item in t.GetNestedType)
        }

    }
}
