using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_FragDirButton : MonoBehaviour
{
    [SerializeField] private GameObject menuBridge;
    [SerializeField] private UI_PupuAlchemist UI_alchemist;
    [SerializeField] private FragmentDirection fragmentType;

    private Button m_button;

    void Awake(){
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnClick);
    }
    void OnDestroy(){
        m_button.onClick.RemoveListener(OnClick);
    }
    void OnClick()=>UI_alchemist.SelectFragmentDirection(fragmentType, this);
    public void OnSelect(){
        menuBridge.SetActive(true);
    }
    public void OnDeselect(){
        menuBridge.SetActive(false);
    }
}
