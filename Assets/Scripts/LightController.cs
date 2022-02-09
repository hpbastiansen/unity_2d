using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class LightController : MonoBehaviour
{
    public Light2D explosionLight;
    public Light2D explosionLight_Ground;

    public float explosionLightIntensity;
    public float explosionLightIntensity_Ground;


    void Start()
    {
        DOVirtual.Float(0, explosionLightIntensity, 0.1f, ChangeLight).OnComplete(() => DOVirtual.Float(explosionLightIntensity, 0, .1f, ChangeLight));
        DOVirtual.Float(0, explosionLightIntensity_Ground, 0.1f, ChangeLight_Ground).OnComplete(() => DOVirtual.Float(explosionLightIntensity_Ground, 0, .1f, ChangeLight_Ground));
    }

    void ChangeLight(float x)
    {
        explosionLight.intensity = x;
    }
    void ChangeLight_Ground(float x)
    {
        explosionLight_Ground.intensity = x;
    }
}
