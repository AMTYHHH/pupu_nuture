using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AutoFakeOutline : MonoBehaviour
{
    //Texture2D srcImg;
    [SerializeField] int kernelSize = 3;
    [SerializeField] Color outlineColor = Color.white;
    Color outlineColorBak;
    [SerializeField] bool smoothOutline;
    [SerializeField] bool maxinumOutline;
    [SerializeField] float gammaEnhance = -1;
    [SerializeField] float alphaThresh = 0.1f;

    [SerializeField] FilterMode filterMode;

    [SerializeField] bool recursive = false;


    [Header("Blur")]
    [SerializeField] int kernelSizeBlur;
    [SerializeField] bool useGaussianKernel;
    [SerializeField] float sigma;


    List<GameObject> listOutlines = new List<GameObject>();

    public void StartGenearte()
    {
        foreach (GameObject go in listOutlines)
        {
            Destroy(go);
        }
        listOutlines.Clear();
        if (recursive)
        {
            foreach (SpriteRenderer sr in this.GetComponentsInChildren<SpriteRenderer>())
            {
                Texture2D srcImg = sr.sprite.texture;
                Rect rect = sr.sprite.rect;
                rect.y = srcImg.height - rect.height - rect.y;
                srcImg = DuplicateTexture(srcImg, rect);

                GenerateOutline(srcImg, sr.transform);
            }
        }
        else
        {
            SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
            if (sr)
            {
                Texture2D srcImg = sr.sprite.texture;
                Rect rect = sr.sprite.rect;
                rect.y = srcImg.height - rect.height - rect.y;
                srcImg = DuplicateTexture(srcImg, rect);

                GenerateOutline(srcImg, this.transform);
            }
        }
    }

    private float[] GetGaussianKernel(int kernelSize, float sigma)
    {
        float[] weights = new float[kernelSize * kernelSize];
        int center = kernelSize - kernelSize / 2;

        for (int i = 0; i < kernelSize; i++)
        {
            for (int j = 0; j < kernelSize; j++)
            {
                weights[j * kernelSize + i] = (1 / (2 * Mathf.PI * sigma * sigma)) * Mathf.Exp(-((i - center) * (i - center) + (j - center) * (j - center)) / (2 * sigma * sigma));
            }
        }

        return weights;
    }

    private Texture2D DuplicateTexture(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
        source.width,
        source.height,
        0,
        RenderTextureFormat.Default,
        RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    private Texture2D DuplicateTexture(Texture2D source, Rect rect)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
        source.width,
        source.height,
        0,
        RenderTextureFormat.Default,
        RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D((int)rect.width, (int)rect.height);
        readableText.ReadPixels(rect, 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }

    private void GenerateOutline(Texture2D srcImg, Transform parent)
    {
        Texture2D tarImg;
        int hsize = kernelSize / 2;
        tarImg = new Texture2D(srcImg.width + hsize * 2, srcImg.height + hsize * 2, TextureFormat.RGBA32, false);
        tarImg.filterMode = filterMode;
        Color[] tarColors = new Color[tarImg.width * tarImg.height];
        //float[] smoothKernel = GetGaussianKernel(kernelSize, sigma);
        for (int x = 0; x < tarImg.width; x++)
        {
            for (int y = 0; y < tarImg.height; y++)
            {
                float value = 0.0f; // use for calc tar alpha
                float count = 0;
                for (int kx = -hsize; kx < hsize + 1; kx++)
                {
                    for (int ky = -hsize; ky < hsize + 1; ky++)
                    {
                        int tx = x + kx - hsize;
                        int ty = y + ky - hsize;
                        if (tx <= 0 || tx >= srcImg.width || ty <= 0 || ty >= srcImg.height)
                        {
                            continue;
                        }

                        Color srcC = srcImg.GetPixel(tx, ty);
                        if (smoothOutline)
                        {
                            value += srcC.a;
                            count += 1;
                        }
                        else if (maxinumOutline)
                        {
                            if (value < srcC.a)
                            {
                                value = srcC.a;
                                count = 1;
                            }
                        }
                        else
                        {
                            if (srcC.a > alphaThresh)
                            {
                                value += 1;
                            }
                        }

                    }
                }

                if (smoothOutline || maxinumOutline)
                {
                    Color tarColor = outlineColor;
                    tarColor.a = value / count;
                    if (gammaEnhance > 0)
                    {
                        tarColor.a = Mathf.Pow(value / count, gammaEnhance);

                    }
                    tarColors[y * tarImg.width + x] = tarColor;
                }
                else
                {
                    if (value > 0)
                    {
                        tarColors[y * tarImg.width + x] = outlineColor;
                        //tarImg.SetPixel(x + hsize, y + hsize, Color.white);
                    }
                    else
                    {
                        tarColors[y * tarImg.width + x] = outlineColorBak;
                        //tarImg.SetPixel(x + hsize, y + hsize, new Color(0, 0, 0, 0));
                    }
                }

                //if (flag)
                //{
                //    tarColors[y * tarImg.width + x] = outlineColor;
                //    //tarImg.SetPixel(x + hsize, y + hsize, Color.white);
                //}
                //else
                //{
                //    tarColors[y * tarImg.width + x] = outlineColorBak;
                //    //tarImg.SetPixel(x + hsize, y + hsize, new Color(0, 0, 0, 0));
                //}
            }
        }

        if (kernelSizeBlur > 0)
        {
            hsize = kernelSizeBlur / 2;
            float[] kernel;
            if (useGaussianKernel)
            {
                kernel = GetGaussianKernel(kernelSizeBlur, sigma);
            }
            else
            {
                kernel = new float[kernelSizeBlur * kernelSizeBlur];
                for (int i = 0; i < kernel.Length; i++)
                {
                    kernel[i] = 1;
                }
            }
            for (int x = 0; x < tarImg.width; x++)
            {
                for (int y = 0; y < tarImg.height; y++)
                {
                    float sum_a = 0f;
                    float sum_weight = 0;
                    for (int kx = -hsize; kx < hsize + 1; kx++)
                    {
                        for (int ky = -hsize; ky < hsize + 1; ky++)
                        {
                            int tx = x + kx;
                            int ty = y + ky;
                            if (tx <= 0 || tx >= tarImg.width || ty <= 0 || ty >= tarImg.height)
                            {
                                continue;
                            }

                            Color srcC = tarColors[ty * tarImg.width + tx];
                            int kIdx = (ky + hsize) * kernelSizeBlur + kx + hsize;
                            sum_a += srcC.a * kernel[kIdx];
                            sum_weight += kernel[kIdx];
                        }
                    }

                    float tar_a = sum_a / sum_weight;
                    tarColors[y * tarImg.width + x].a = tar_a;

                }
            }
        }
        tarImg.SetPixels(tarColors);
        tarImg.Apply();

        GameObject newGo = new GameObject("outline");
        listOutlines.Add(newGo);
        newGo.transform.parent = parent;
        newGo.transform.localPosition = new Vector3(0, 0, 0);
        newGo.transform.localScale = new Vector3(1, 1, 1);
        SpriteRenderer newSr = newGo.AddComponent<SpriteRenderer>();
        newSr.sprite = Sprite.Create(tarImg, new Rect(0, 0, tarImg.width, tarImg.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    private void Awake()
    {
        outlineColorBak = outlineColor;
        outlineColorBak.a = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartGenearte();
    }

    // Update is called once per frame
    void Update()
    {


    }
}


