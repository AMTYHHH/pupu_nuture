using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_MapButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mapText;
    private UI_PupuAlchemist UI_alchemist;
    private Button m_button;
    void Awake(){
        UI_alchemist = FindObjectOfType<UI_PupuAlchemist>();

        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnClick);
    }
    void OnDestroy(){
        m_button.onClick.RemoveListener(OnClick);
    }
    public void Initialize(string mapName)=>mapText.text = mapName;
    public void OnClick(){
        UI_alchemist.SelectMap(mapText.text);
    }
}
