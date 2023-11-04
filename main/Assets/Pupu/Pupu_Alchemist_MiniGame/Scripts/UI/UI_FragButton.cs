using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_FragButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonName;
    [SerializeField] private TextMeshProUGUI amountName;
    public void Initialize(string fragName, int fragAmount){
        buttonName.text = fragName;
        amountName.text = $"x{fragAmount}";
    }
}
