using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Arrow : MonoBehaviour
{
[Header("Pupu定位")]
    [SerializeField] private Transform mapCenter;
    [SerializeField] private Transform pupuCharacterCenter;

    private RectTransform arrowTrans;

    void Start()=>arrowTrans = GetComponent<RectTransform>();
    void Update()
    {
        Vector3 diff = pupuCharacterCenter.position - mapCenter.position;        
        diff.z = 0;

        float angle = Mathf.Atan2(diff.y, diff.x)*Mathf.Rad2Deg;
        arrowTrans.rotation = Quaternion.Euler(0, 0, angle);
        Vector3 anchoredPos = arrowTrans.rotation * Vector3.right * 500;
        anchoredPos.x = Mathf.Clamp(anchoredPos.x, -290, 290);
        anchoredPos.y = Mathf.Clamp(anchoredPos.y, -290, 290);
        arrowTrans.anchoredPosition = anchoredPos;
    }
}
