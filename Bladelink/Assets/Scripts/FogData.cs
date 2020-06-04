using System;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class FogData : Singleton<FogData>
{
    private Fog[] fogs;

    [Header("Attributes")]
    public float density = 0.1f;
    public float fogBack = 10f;
    public float fogFront = 0f;

    [Header("Colors")]
    [Tooltip("The left value influences the top of the sprite, the right value influences the bottom")]
    public Gradient fogGradient;
    public Color tint = Color.white;


    private void Awake()
    {
        fogs = FindObjectsOfType<Fog>();
    }

    private void OnValidate()
    {
        if (fogs != null)
        {
            foreach (Fog fog in fogs)
            {
                fog.SetAttributes();
            }
        }
    }

}