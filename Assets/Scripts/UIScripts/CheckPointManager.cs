using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [Header("World 1")]
    public int W1C1;
    public string W1C1Scenename;
    public int W1C2;
    public string W1C2Scenename;
    public int W1C3;
    public string W1C3Scenename;
    public int W1C4;
    public string W1C4Scenename;
    public int W1C5;
    public string W1C5Scenename;
    public int W1C6;
    public string W1C6Scenename;

    public List<string> W1Scenes;
    public List<string> W2Scenes;
    public List<string> W3Scenes;
    [Header("Ammo and Health")]
    public HealthAndAmmoForStage AmmoAndHealth;


    // Start is called before the first frame update
    void Start()
    {
        if (W1C1 == 0 && W1C2 == 0 && W1C3 == 0 && W1C4 == 0 && W1C5 == 0 && W1C6 == 0)
        {
            W1C1 = 1;
        }
        W1Scenes.Add("LEVEL1_NOT");
    }
    public void AddCheckPointW1(string _name)
    {
        W1Scenes.Add(_name);
    }
    public void AddCheckPointW2(string _name)
    {
        W2Scenes.Add(_name);
    }
    public void AddCheckPointW3(string _name)
    {
        W3Scenes.Add(_name);
    }
}
