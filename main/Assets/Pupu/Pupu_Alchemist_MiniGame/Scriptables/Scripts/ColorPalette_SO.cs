using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pupu/ColorPalette_SO")]
public class ColorPalette_SO : ScriptableObject
{
    [SerializeField] private List<ColorPalette> colors;
    public Color GetColorByName(ColorType colorType){return colors.Find(x=>x.colorType == colorType).color;}
}

[System.Serializable]
public class ColorPalette{
    public ColorType colorType;
    [ColorUsage(false)] public Color color;
}

public enum ColorType{Pink, Cyan, Blue, Purple, Red, Crimson, Green}
