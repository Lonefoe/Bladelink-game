using UnityEngine;


public class Fog : MonoBehaviour
{
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;

        SetAttributes();

    }

    public void SetAttributes()
    {
        Vector4 gradientBottom = FogData.Instance.fogGradient.colorKeys[1].color;
        gradientBottom.w = FogData.Instance.fogGradient.alphaKeys[1].alpha;
        Debug.Log(gradientBottom);

        Vector4 gradientTop = FogData.Instance.fogGradient.colorKeys[0].color;
        gradientTop.w = FogData.Instance.fogGradient.alphaKeys[0].alpha;
        Debug.Log(gradientTop);

        material.SetVector("Color_EC11A50F", gradientBottom);
        material.SetVector("Color_5EAD343A", gradientTop);
        material.SetFloat("Vector1_8E1F966D", FogData.Instance.density);
        material.SetVector("Color_681391DF", FogData.Instance.tint);
        material.SetFloat("Vector1_412E45B4", FogData.Instance.fogBack);
        material.SetFloat("Vector1_279BB8A8", FogData.Instance.fogFront);
    }

}