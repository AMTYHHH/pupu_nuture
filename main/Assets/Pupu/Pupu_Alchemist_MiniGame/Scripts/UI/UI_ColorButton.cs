using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_ColorButton : MonoBehaviour
{
    [SerializeField] private UI_PupuAlchemist UI_alchemist;
    [SerializeField] private ColorType colorType;
    private Button m_button;
    void Awake(){
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnClick);
    }
    void OnDestroy(){
        m_button.onClick.RemoveListener(OnClick);
    }
    public void OnClick(){
        UI_alchemist.SelectColor(colorType, this);
    }
    public void OnSelect(){
        transform.localScale = Vector3.one * 1.3f;
    }
    public void OnDeselect(){
        transform.localScale = Vector3.one;
    }
}
