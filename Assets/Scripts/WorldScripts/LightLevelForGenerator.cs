using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// This script is used to set the light level in the whole stage by the Generator in stage 3.
public class LightLevelForGenerator : MonoBehaviour
{
    public Puzzle1 Puzzle1Script;
    public Light2D MyLight;
    public bool IsGobal;
    public bool IsGobalBackground;
    public bool NormalLight;

    /// Called before the first frame.
    void Start()
    {
        Puzzle1Script = FindObjectOfType<Puzzle1>();
        MyLight = GetComponent<Light2D>();
    }

    /// Called every frame.
    /** If the puzzle is complete, set the light level according to the GeneratorPowerValue, otherwise the light is turned off. */
    void Update()
    {
        if (NormalLight)
        {
            if (Puzzle1Script.GeneratorCanBeStarted == true && Puzzle1Script.Door1Open == true)
            {
                MyLight.intensity = Puzzle1Script.GeneratorPowerValue / 100;
                if (MyLight.intensity > .7f)
                {
                    MyLight.intensity = .7f;
                }
            }
            if (Puzzle1Script.GeneratorCanBeStarted == true && Puzzle1Script.Door1Open == false)
            {
                MyLight.intensity = .7f;
            }
        }

        if (IsGobal)
        {
            if (Puzzle1Script.GeneratorCanBeStarted == true)
            {
                MyLight.intensity = .07f;
            }
            else
            {
                MyLight.intensity = 0f;
            }
        }
        if (IsGobalBackground)
        {
            if (Puzzle1Script.GeneratorCanBeStarted == true)
            {
                MyLight.intensity = .04f;
            }
            else
            {
                MyLight.intensity = 0f;
            }
        }
    }
}
