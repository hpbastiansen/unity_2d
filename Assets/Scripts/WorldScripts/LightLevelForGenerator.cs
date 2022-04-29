using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightLevelForGenerator : MonoBehaviour
{
    public Puzzle1 Puzzle1Script;
    public Light2D MyLight;
    public bool IsGobal;
    // Start is called before the first frame update
    void Start()
    {
        Puzzle1Script = Object.FindObjectOfType<Puzzle1>();
        MyLight = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Puzzle1Script.GeneratorCanBeStarted == true)
        {
            MyLight.intensity = Puzzle1Script.GeneratorPowerValue / 100;
            if (MyLight.intensity > .7f)
            {
                MyLight.intensity = .7f;
            }
        }
        else
        {
            if (Puzzle1Script.Door1Open == true)
            {
                MyLight.intensity = 0;
            }
            else
            {
                MyLight.intensity = .7f;
            }


        }

        if (IsGobal)
        {
            if (Puzzle1Script.GeneratorCanBeStarted == true)
            {
                MyLight.intensity = 0.07f;
            }
            else
            {
                if (Puzzle1Script.Door1Open == true)
                {
                    MyLight.intensity = 0;
                }
                else
                {
                    MyLight.intensity = .07f;
                }
            }

        }

    }
}
