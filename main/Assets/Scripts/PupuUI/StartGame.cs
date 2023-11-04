using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUI;
using System.Reflection;
using System;
using static UnityEditor.Progress;
using GameLogic;

public class StartGame : MonoBehaviour
{
    GComponent mainContentPane;
    GComponent guideContentPane;

    Controller pageState;

    // Start is called before the first frame update
    void Start()
    {
        // 采用Persistent场景下的prefab进行数据存储


        //UIPackage.AddPackage("UI/Main");
        //UIPackage.AddPackage("UI/Common");
        //UIPackage.AddPackage("UI/Guide");


        //mainContentPane = UIPackage.CreateObject("Main", "MainUI").asCom;
        //guideContentPane = UIPackage.CreateObject("Guide", "GuideUI").asCom;

        //GRoot.inst.AddChild(mainContentPane);
        //GRoot.inst.AddChild(guideContentPane);

        //var guideWin = new GuideDialog();
        //guideWin.Show();

        //TestVaule mm = new TestVaule();
        //Type T = mm.GetType();
        //var FileSingleInfo = T.GetProperty("m_Value1", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        //Debug.Log("FileSingleInfo:" + FileSingleInfo.GetValue(mm));
        ////string Temp = "ppp";
        //var list = T.GetProperties();
        //foreach (var item in list)
        //{
        //    Debug.Log("PropertyTypeName:" + item.Name + " className:" + item.GetType().Name);
        //}

        UIConfig.bringWindowToFrontOnClick = false;
        UIManager.OpenDialog(DialogType.MainDialog);
        MainGame.StartGame();

        //这里是无法赋值的，但是可以获取，m_Value2 m_Value3 均可以通过GetValue,及SetValue 赋值
        //UIManager.ShowCommonConfirmDialog();
    }

    private void OnClickBgBtn()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MainGame.Update();
    }
}

public class TestVaule : TestParentValue
{

    /// <summary>
    /// 不可赋值
    /// </summary>
    public string m_Value1 { get { return "111"; } }

    /// <summary>
    /// 可赋值
    /// </summary>
    public string m_Value2 { get; private set; }
    private GButton settingBtn;
    private GButton guideBtn;
    private GButton infoBtn = null;
    private GButton roomBtn;
    /// <summary>
    /// 可赋值
    /// </summary>
    public string m_Value3
    {
        get { return m_Value3; }
        set { m_Value3 = value; }
    }
}

public class TestParentValue
{
    /// <summary>
    /// 不可赋值
    /// </summary>
    public string m_ValueParent1 { get { return "111"; } }

    /// <summary>
    /// 可赋值
    /// </summary>
    public string m_ValueParent2 { get; private set; }

}
