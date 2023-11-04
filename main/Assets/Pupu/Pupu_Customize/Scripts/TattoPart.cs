using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[System.Serializable]
public class TattoData{
    public Color tattoColor;
    public string tattoMapName;
    public Vector2Int tattoOffset;
    public Vector2Int tattoSize;
}
public class TattoPart : MonoBehaviour
{
    public SpriteRenderer bodyRenderer;
    [SerializeField] private TattoData tattoData;

    private const string _SpriteRectName = "_SpriteRect";
    private const string _TattoMapModeName = "_TattoMapMode";
    private const string _TattoColorName = "_TattoColor";
    private const string _TattoTexName = "_TattoTex";
    private const string _FlipXName = "_FlipX";

    void Start(){
        Vector4 rect;
        rect.x = bodyRenderer.sprite.textureRect.xMin;
        rect.y = bodyRenderer.sprite.textureRect.yMin;
        rect.z = bodyRenderer.sprite.textureRect.width;
        rect.w = bodyRenderer.sprite.textureRect.height;
        
        bodyRenderer.material.SetVector(_SpriteRectName, rect);
        bodyRenderer.material.SetFloat(_FlipXName, transform.localScale.x > 0?0:1);
    }
    public void OnSelectBody(){
        bodyRenderer.material.SetFloat(_TattoMapModeName, 1);
    }
    public void OnDeselectBody(){
        bodyRenderer.material.SetFloat(_TattoMapModeName, 0);
    }
    public void PrintTatto(Texture2D _tattoTex, Color _tattoColor){
        Texture tex = bodyRenderer.material.GetTexture(_TattoTexName);
        if(tex != null){
            Destroy(tex);
            Debug.Log("Need to clear Tex");
        }

        bodyRenderer.material.SetFloat(_TattoMapModeName, 0);
        bodyRenderer.material.SetTexture(_TattoTexName, _tattoTex);
        bodyRenderer.material.SetColor(_TattoColorName, _tattoColor);
    }
    public void UpdateTattoData(string _tattoMapName, Color _tattoColor, Vector2Int offset, Vector2Int size){
        tattoData.tattoMapName = _tattoMapName;
        tattoData.tattoColor = _tattoColor;
        tattoData.tattoOffset = offset;
        tattoData.tattoSize = size;
    }
}
