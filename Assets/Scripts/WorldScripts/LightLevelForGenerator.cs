using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightLevelForGenerator : MonoBehaviour
{
    public Puzzle1 Puzzle1Script;
    public Light2D MyLight;
    public bool IsGobal;
    public bool IsGobalBackground;
    public bool NormalLight;
    // Start is called before the first frame update
    void Start()
    {
        Puzzle1Script = Object.FindObjectOfType<Puzzle1>();
        MyLight = GetComponent<Light2D>();
    }

    // Update is called once per frame
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
