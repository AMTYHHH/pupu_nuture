using FairyGUI;
using GameUI;
using System;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDialogData
{
    public string dialogName = "";
    public string pkgName = "";
    public string dialogResName = "";
    public string sceneName = "";

    public UIDialogData(string dialogName, string pkgName, string dialogResName, string sceneName = "")
    {
        // Dialog对应类型名称
        this.dialogName = dialogName;
        // Dialog所在UI包包名
        this.pkgName = pkgName;
        // Dialog资源名（FGUI中的名称）
        this.dialogResName = dialogResName;
        // Dialog对应Scene名
        this.sceneName = sceneName;
    }
}

// UI界面管理，暂时可满足需求后弦需优化
public class UIManager
{
    private static Dictionary<string, UIDialogData> dialogDataDic = null;

    private static Dictionary<string, Dialog> dialogDic = new Dictionary<string, Dialog>();
    private static Dictionary<string, int> pkgCountDic = new Dictionary<string, int>();

    private static Stack<Dialog> dialogStack = new Stack<Dialog>();

    public static void InitDialogInfoList()
    {
        dialogDataDic = new Dictionary<string, UIDialogData>();
        // key:dialogName，一个class对应一个UI界面
        dialogDataDic["MainDialog"] = new UIDialogData("MainDialog", "Main", "MainUI", "Persistance");
        dialogDataDic["GuideDialog"] = new UIDialogData("GuideDialog", "Guide", "GuideUI");
        dialogDataDic["CommonConfirmDialog"] = new UIDialogData("CommonConfirmDialog", "Common", "CommonConfirmUI");
        dialogDataDic["RoomLobbyDialog"] = new UIDialogData("RoomLobbyDialog", "RoomLobby", "RoomLobbyUI", "FurnitureCustomize");
        dialogDataDic["RoomRoleDialog"] = new UIDialogData("RoomRoleDialog", "RoomRole", "RoomRoleUI", "PupuCustomize");
        dialogDataDic["RoomAlchemistDialog"] = new UIDialogData("RoomAlchemistDialog", "RoomAlchemist", "RoomAlchemistUI", "PupuAlchemist");
        dialogDataDic["UseGoodsDialog"] = new UIDialogData("UseGoodsDialog", "RoomLobby", "UseGoodsUI");
        dialogDataDic["RoomCommonDialog"] = new UIDialogData("RoomCommonDialog", "RoomCommon", "RoomCommonUI");

        //dialogDic = new Dictionary<string, Dialog>();
        //pkgCountDic = new Dictionary<string, int>();
        UIPackage.AddPackage("UI/Common");
    }

    /// <summary>
    /// 打开窗口
    /// </summary>
    /// <param name="dialogName">窗口名称</param>
    public static void OpenDialog(DialogType dialogType)
    {
        var dialogName = dialogType.ToString();
        if (dialogDataDic == null)
            UIManager.InitDialogInfoList();

        dialogDataDic.TryGetValue(dialogName, out var dialogData);
        if (dialogData == null)
            return;

        OpenScene(dialogData.sceneName);

        Debug.Log("OpenDialog:" + dialogType.ToString());
        Dialog dialog = null;
        dialogDic.TryGetValue(dialogName, out dialog);
        if (dialog != null)
        {
            dialog.Show();
            return;
        }

        string className = "GameUI." + dialogName;
        Type t = Type.GetType(className);

        Debug.Log("OpenDialog:" + dialogType.ToString() + " IS Null :" + t != null);
        //创建实例对象
        if (t != null && t.IsSubclassOf(typeof(Dialog)))
        {
            UIManager.AddPackage(dialogData.pkgName);
            //var dialog = Activator.CreateInstance(t) as Dialog;
            dialog = t.Assembly.CreateInstance(className) as Dialog;
            dialog.contentPane = UIPackage.CreateObject(dialogData.pkgName, dialogData.dialogResName).asCom;
            Debug.Log("OpenDialog:" + dialogType.ToString() + " contentPane Create Sucess");
            dialog.Show();

            dialogDic[dialogName] = dialog;
        }

    }

    /// <summary>
    /// 关闭窗口
    /// </summary>
    /// <param name="dialogName">窗口名称</param>
    public static void CloseDialog(DialogType dialogType)
    {
        var dialogName = dialogType.ToString();
        if (dialogDic == null)
            return ;

        dialogDataDic.TryGetValue(dialogName, out var dialogData);
        if (dialogData == null)
            return;

        CloseScene(dialogData.sceneName);

        dialogDic.TryGetValue(dialogName, out var dialog);
        if (dialog == null)
            return;

        dialog.Hide();
        //dialog
    }

    public static void OpenScene(string sceneName)
    {
        if (sceneName == null || sceneName == "")
            return;

        var scene = SceneManager.GetSceneByName(sceneName);
        if (scene != null)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public static void CloseScene(string sceneName)
    {
        if (sceneName == null || sceneName == "")
            return;

        var scene = SceneManager.GetSceneByName(sceneName);
        if (scene != null)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
    }

    public static void CloseAllDialog()
    {
        UIPackage.RemoveAllPackages();
    }

    public static Dialog GetDialog(string dialogName)
    {
        dialogDic.TryGetValue(dialogName, out var dialog);
        return dialog;
    }

    private static void AddPackage(string pkgName)
    {
        var pkg = UIPackage.AddPackage("UI/" + pkgName);
        // 以支持子包导入
        foreach (var childPkgList in pkg.dependencies)
        {
            UIPackage.AddPackage("UI/" + childPkgList["name"]);
        }
    }

    private static void RemovePackage(string pkgName)
    {

    }

    /// <summary>
    /// 打开通用确认窗口
    /// </summary>
    /// <param name="okCallback"></param>
    /// <param name="cancelCallback"></param>
    public static void ShowCommonConfirmDialog(string content, EventCallback0 okCallback = null, EventCallback0 cancelCallback = null)
    {
        var dialogType = DialogType.CommonConfirmDialog;
        string dialogName = dialogType.ToString();
        UIManager.OpenDialog(dialogType);

        dialogDic.TryGetValue(dialogName, out var value);
        var dialog = value as CommonConfirmDialog;
        dialog.SetConfirmContent(content);
        dialog.SetEventCallback(okCallback, cancelCallback);
    }


}
