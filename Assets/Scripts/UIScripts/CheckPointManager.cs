using System.Collections.Generic;
using UnityEngine;

/// This script manages which stages the player has reached throughout the whole game.
public class CheckPointManager : MonoBehaviour
{
    [Header("Tutorial")]
    public bool IsTutorialDone;

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

    /// Called before the first frame.
    /** If no stage is marked as the current stage, set the first stage as current stage. */
    void Start()
    {
        if (W1C1 == 0 && W1C2 == 0 && W1C3 == 0 && W1C4 == 0 && W1C5 == 0 && W1C6 == 0)
        {
            W1C1 = 1;
        }
    }

    /// Add a new stage to W1Scenes.
    public void AddCheckPointW1(string _name)
    {
        W1Scenes.Add(_name);
    }

    /// Add a new stage to W2Scenes.
    public void AddCheckPointW2(string _name)
    {
        W2Scenes.Add(_name);
    }

    /// Add a new stage to W3Scenes.
    public void AddCheckPointW3(string _name)
    {
        W3Scenes.Add(_name);
    }
}
