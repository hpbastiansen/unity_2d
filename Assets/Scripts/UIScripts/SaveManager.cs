using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// This script manages the save state of the game. Loading on game start, saving on scene start and game quit.
public class SaveManager : MonoBehaviour
{
    public CheckPointManager CheckPointManagerScript;
    public TokenManager TokenManagerScript;
    public Slider SFXSoundSettings;
    public Slider MusicSoundSettings;
    public WeaponController WeaponControllerScript;

    [Header("Default variables")]
    public List<string> defaultListForCheckpointsW1 = new List<string>();
    public List<GameObject> defaultListForTokensOwned;

    /// Called at initialization, before all objects Start() methods.
    /** Set default values and load the saved game at game start. */
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        defaultListForCheckpointsW1.Add("STAGE1");
        Load();
    }

    /// Save everything.
    public void Save()
    {
        ES3.Save("W1Scenes", CheckPointManagerScript.W1Scenes);
        ES3.Save("SFXSoundVolume", SFXSoundSettings.value);
        ES3.Save("MusicSoundVolume", MusicSoundSettings.value);
        ES3.Save("CactiInt", TokenManagerScript.CactiDestoyed);
        ES3.Save("ShrubsInt", TokenManagerScript.ShrubsDestoyed);
    }

    /// Load everything.
    public void Load()
    {
        CheckPointManagerScript.W1Scenes = ES3.Load("W1Scenes", defaultListForCheckpointsW1);
        SFXSoundSettings.value = ES3.Load("SFXSoundVolume", 0.5f);
        MusicSoundSettings.value = ES3.Load("MusicSoundVolume", 0.5f);
        TokenManagerScript.CactiDestoyed = ES3.Load("CactiInt", 0);
        TokenManagerScript.ShrubsDestoyed = ES3.Load("ShrubsInt", 0);
    }

    /// Save the game before the application quits.
    private void OnApplicationQuit()
    {
        Save();
    }

    /// Save the game on scene load.
    public void OnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        Save();
    }
}
