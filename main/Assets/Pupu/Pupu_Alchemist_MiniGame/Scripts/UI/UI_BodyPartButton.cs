using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_BodyPartButton : MonoBehaviour
{
    [SerializeField] private UI_PupuAlchemist UI_alchemist;
    [SerializeField] private BodyPartType bodyPartType;
    [SerializeField] private Image buttonImage;
    [SerializeField] private Color NormalColor;
    [SerializeField] private Color SelectColor;
    private Button m_button;
    void Awake(){
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnClick);
    }
    void OnDesotry(){
        m_button.onClick.RemoveListener(OnClick);
    }
    public void OnClick(){
        UI_alchemist.SelectBodyPart(bodyPartType, this);
    }
    public void OnSelect(){
        buttonImage.color = SelectColor;
    }
    public void OnDeselect(){
        buttonImage.color = NormalColor;
    }

}
