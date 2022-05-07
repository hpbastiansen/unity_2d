using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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



    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        defaultListForCheckpointsW1.Add("STAGE1");
        Load();
    }

    public void Save()
    {
        ES3.Save("W1Scenes", CheckPointManagerScript.W1Scenes);
        ES3.Save("SFXSoundVolume", SFXSoundSettings.value);
        ES3.Save("MusicSoundVolume", MusicSoundSettings.value);
        ES3.Save("CactiInt", TokenManagerScript.CactiDestoyed);
        ES3.Save("ShrubsInt", TokenManagerScript.ShrubsDestoyed);
    }

    public void Load()
    {
        CheckPointManagerScript.W1Scenes = ES3.Load("W1Scenes", defaultListForCheckpointsW1);
        SFXSoundSettings.value = ES3.Load("SFXSoundVolume", 0.5f);
        MusicSoundSettings.value = ES3.Load("MusicSoundVolume", 0.5f);
        TokenManagerScript.CactiDestoyed = ES3.Load("CactiInt", 0);
        TokenManagerScript.ShrubsDestoyed = ES3.Load("ShrubsInt", 0);
    }
    private void OnApplicationQuit()
    {
        Save();
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Save();
    }
}
