using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Texture_SnapShoter
{
    private const string SNAPSHOT_FOLDER_PATH = "/Snapshot/";
    public static Texture2D TakeSnapshot(Camera cam, string fileName, Vector2Int clipOffset, Vector2Int clipSize){
        string folderpath = Application.persistentDataPath + SNAPSHOT_FOLDER_PATH;
        string filePath = folderpath + fileName + ".png";

        if(!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);
        
        var currentCamRT = cam.targetTexture;
        var currentActiveRT = RenderTexture.active;

        Texture2D snapshot = new Texture2D(clipSize.x, clipSize.y, TextureFormat.ARGB32, false, false);

        RenderTexture.active = cam.targetTexture;
        snapshot.ReadPixels(new Rect(clipOffset.x, clipOffset.y, clipSize.x, clipSize.y), 0, 0);
        snapshot.Apply();

        cam.targetTexture = currentCamRT;
        RenderTexture.active = currentActiveRT;

        Debug.Log($"Saving snapshot at {filePath}");
        File.WriteAllBytes(filePath, snapshot.EncodeToPNG());

        return snapshot;
    }
    public static Texture2D TakeSnapshot(Texture2D originalTex, string fileName, Vector2Int clipOffset, Vector2Int clipSize, float DownSampleLevel = 1){
        float multiplier = 1f/DownSampleLevel;

        string folderpath = Application.persistentDataPath + SNAPSHOT_FOLDER_PATH;
        string filePath = folderpath + fileName + ".png";

        if(!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);
        
        Vector2Int realSize = Vector2Int.RoundToInt(multiplier*(Vector2)clipSize);
        Texture2D snapshot = new Texture2D(realSize.x, realSize.y, TextureFormat.ARGB32, false, false);
        snapshot.wrapMode = TextureWrapMode.Clamp;

        if(DownSampleLevel == 1) snapshot.SetPixels(originalTex.GetPixels(clipOffset.x, clipOffset.y, clipSize.x, clipSize.y));
    //根据down sample等级来做采样
        else{
            for(int y=0; y<realSize.y; y++){
                for(int x=0; x<realSize.x; x++){
                    snapshot.SetPixel(x, y, originalTex.GetPixel(Mathf.RoundToInt(x*DownSampleLevel+clipOffset.x), Mathf.RoundToInt(y*DownSampleLevel+clipOffset.y)));
                }
            }
        }
        snapshot.Apply();

        Debug.Log($"Saving snapshot at {filePath}");
        File.WriteAllBytes(filePath, snapshot.EncodeToPNG());

        return snapshot;
    }
    public static Texture2D ReadSnapshot(string fileName){
        string filePath = Application.persistentDataPath + SNAPSHOT_FOLDER_PATH + fileName;

        if(!File.Exists(filePath)) return null;

        Texture2D readTex = new Texture2D(2,2,TextureFormat.ARGB32, 0, false);
        ImageConversion.LoadImage(readTex, File.ReadAllBytes(filePath));

        Debug.Log($"Read snapshot from {filePath}");
        return readTex;
    }
}
