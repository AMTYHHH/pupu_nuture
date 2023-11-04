using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_PupuCustomize : MonoBehaviour
{
    [SerializeField] private TattoMap tattoMapControl;
    [SerializeField] private TMP_InputField inputField;
    private float currentScaleDownFactor = 1;
    void Awake(){
        inputField.onValueChanged.AddListener(OnChangeScaleFactor);
    }
    public void ConfirmTatto()=>tattoMapControl.ConfirmCustomize(currentScaleDownFactor);
    public void UnloadMap()=>tattoMapControl.UnloadMapFile();
    public void LoadMap(string mapName)=>tattoMapControl.LoadMapFile(mapName);
    public void OnChangeScaleFactor(string input){
        if(input == string.Empty){
            currentScaleDownFactor = 1;
            return;
        }
        
        float value = float.Parse(input);
        currentScaleDownFactor = 1.0f/value;
    }
}
