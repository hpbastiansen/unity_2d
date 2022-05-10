//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*This script was included in a pre made 2D pixel game package made by Unity Technologies. And thus not listed as a source for this project.*/
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

/// This scripts controls light animation and intensity on spesific 2D Lights instantiated at bullet explosions.
/** NOTE: This script was included in a pre made 2D pixel game package made by Unity Technologies themselves. And thus no spesific source listed. */
public class LightController : MonoBehaviour
{
    public Light2D ExplosionLight;
    public Light2D ExplosionLight_Ground;

    public float ExplosionLightIntensity;
    public float ExplosionLightIntensity_Ground;

    /// Start methods run once when enabled.
    /** Start is called on the frame when a script is enabled just before any of the Update methods are called the first time. */
    /*! In the Start function the Tweens are defined. */
    void Start()
    {
        DOVirtual.Float(0, ExplosionLightIntensity, 0.1f, ChangeLight).OnComplete(() => DOVirtual.Float(ExplosionLightIntensity, 0, .1f, ChangeLight));
        DOVirtual.Float(0, ExplosionLightIntensity_Ground, 0.1f, ChangeLight_Ground).OnComplete(() => DOVirtual.Float(ExplosionLightIntensity_Ground, 0, .1f, ChangeLight_Ground));
    }

    /// The ChangeLight changes the light intensity of the light source that affect everything but the ground.
    void ChangeLight(float x)
    {
        ExplosionLight.intensity = x;
    }
    /// The ChangeLight changes the light intensity of the light source that only affect the ground.
    void ChangeLight_Ground(float x)
    {
        ExplosionLight_Ground.intensity = x;
    }
}
