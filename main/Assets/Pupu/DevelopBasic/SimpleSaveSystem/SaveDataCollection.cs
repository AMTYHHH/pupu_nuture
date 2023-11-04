using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class GlobalSaveData{
    public string currentScene = string.Empty;
    public int MaxLevelIndex = 0;
}
[Serializable]
public class SettingSaveData{

}
[Serializable]
public class PlayerSaveData{
    public Vector3 playerPos;
    public Quaternion playerRot;
    public Quaternion camRot;
}