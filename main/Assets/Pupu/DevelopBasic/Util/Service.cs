using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class Service{
    //Layer
    public static LayerMask UI_Layer = LayerMask.NameToLayer("UI");
    public static LayerMask Back_Layer = LayerMask.NameToLayer("Back_Layer");
    public static LayerMask Mid_Layer = LayerMask.NameToLayer("Mid_Layer");
    public static LayerMask Front_Layer = LayerMask.NameToLayer("Front_Layer");
    public static LayerMask Default_Layer = LayerMask.NameToLayer("Default");

    //Sorting Layer
    public static string Background_SLayer = "Background";
    public static string Default_SLayer = "Default";
    public static string Foreground_SLayer = "Foreground";
#region HelperFunction
    public static T[] FindComponentsOfTypeIncludingDisable<T>(){
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
        var MatchObjects = new List<T> ();

        for(int i=0; i<sceneCount; i++){
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt (i);
            
            var RootObjects = scene.GetRootGameObjects ();

            foreach (var obj in RootObjects) {
                var Matches = obj.GetComponentsInChildren<T> (true);
                MatchObjects.AddRange (Matches);
            }
        }

        return MatchObjects.ToArray ();
    }
#endregion
}