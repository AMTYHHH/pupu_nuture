using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public static class Screenshot_Taker
{
    private const string SCREENSHOT_FOLDER_PATH = "/Screenshot/";
    public static void TakeScreenShot(Camera cam, string fileName, float scaleDownLevel = 1){
        string folderpath = Application.persistentDataPath + SCREENSHOT_FOLDER_PATH;
        string filePath = folderpath + fileName + ".png";

        if(!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);
        
        int width = Mathf.FloorToInt(cam.pixelWidth/scaleDownLevel);
        int height = Mathf.FloorToInt(cam.pixelHeight/scaleDownLevel);

        var currentCamRT = cam.targetTexture;
        var currentActiveRT = RenderTexture.active;

        Texture2D screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false, false);

        RenderTexture.active = cam.targetTexture;
        screenShot.ReadPixels(new Rect(0,0,width, height), 0, 0);
        screenShot.Apply();

        cam.targetTexture = currentCamRT;
        RenderTexture.active = currentActiveRT;

        Debug.Log($"Saving screenshot at {filePath}");
        File.WriteAllBytes(filePath, screenShot.EncodeToPNG());
    }
    public static Texture2D TakeScreenShot(Camera cam, string fileName, Vector2Int clipOffset, Vector2Int clipSize){
        string folderpath = Application.persistentDataPath + SCREENSHOT_FOLDER_PATH;
        string filePath = folderpath + fileName + ".png";

        if(!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);
        
        var currentCamRT = cam.targetTexture;
        var currentActiveRT = RenderTexture.active;

        Texture2D screenShot = new Texture2D(clipSize.x, clipSize.y, TextureFormat.ARGB32, false, false);

        RenderTexture.active = cam.targetTexture;
        screenShot.ReadPixels(new Rect(clipOffset.x, clipOffset.y, clipSize.x, clipSize.y), 0, 0);
        screenShot.Apply();

        cam.targetTexture = currentCamRT;
        RenderTexture.active = currentActiveRT;

        Debug.Log($"Saving screenshot at {filePath}");
        File.WriteAllBytes(filePath, screenShot.EncodeToPNG());

        return screenShot;
    }
    public static Texture2D ReadScreenshot(string fileName){
        string filePath = Application.persistentDataPath + SCREENSHOT_FOLDER_PATH + fileName;

        if(!File.Exists(filePath)) return null;

        Texture2D readTex = new Texture2D(2,2,TextureFormat.ARGB32, 0, false);
        ImageConversion.LoadImage(readTex, File.ReadAllBytes(filePath));

        Debug.Log($"Read screenshot from {filePath}");
        return readTex;
    }
}
