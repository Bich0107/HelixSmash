using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSetter : MonoBehaviour
{
    [SerializeField] List<Color> bottomsColorList;
    [SerializeField] List<Color> topColorList;

    [SerializeField] Material bgMaterial;
    [SerializeField] RawImage bg;
    [SerializeField] Texture2D bgTexture;

    void Awake()
    {
        bgTexture = new Texture2D(1, 2);
        bgTexture.wrapMode = TextureWrapMode.Clamp;
        bgTexture.filterMode = FilterMode.Bilinear;
        ChangeBGColor();
    }

    void SetColor(Color color1, Color color2)
    {
        bgTexture.SetPixels(new Color[] { color1, color2 });
        bgTexture.Apply();
        bg.texture = bgTexture;
    }

    public void ChangeBGColor()
    {
        SetColor(GetRandomColor(topColorList), GetRandomColor(bottomsColorList));
    }

    Color GetRandomColor(List<Color> listColor)
    {
        int index = Random.Range(0, listColor.Count);
        return listColor[index];
    }
}
