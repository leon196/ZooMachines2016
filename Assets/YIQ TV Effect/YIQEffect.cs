using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
public class YIQEffect : ImageEffectBase
{
    public AnimationCurve yCurve;
    public AnimationCurve iqCurve;
    public float distance = 1;

    void SetCurves()
    {
        /*
        var materialProperty = new MaterialPropertyBlock();

        materialProperty.SetFloatArray("arrayName", floatArray);
        gameObject.GetComponent<Renderer>().SetPropertyBlock(materialProperty);

        material.set*/

        float[] yValues = new float[10];
        float[] iqValues = new float[10];

        for (int i = 0; i < 10; i++)
        {
            yValues[i] = yCurve.Evaluate((float)i / 10);
            iqValues[i] = iqCurve.Evaluate((float)i / 10);
        }

        //material.SetFloatArray("yCurve", yValues);
        //material.SetFloatArray("iqCurve", iqValues);

        Shader.SetGlobalFloatArray("_YCurve", yValues);
        Shader.SetGlobalFloatArray("_IQCurve", iqValues);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetCurves();

        material.SetFloat("_Distance", distance);

        Graphics.Blit(source, destination, material);
    }
}
