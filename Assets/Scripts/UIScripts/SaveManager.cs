using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public CheckPointManager CheckPointManagerScript;
    public TokenManager TokenManagerScript;
    public Slider SFXSoundSettings;
    public Slider MusicSoundSettings;



    [Header("Default variables")]
    public List<string> defaultListForCheckpointsW1 = new List<string>();
    public List<GameObject> defaultListForTokensOwned;



    // Start is called before the first frame update
    void Awake()
    {
        defaultListForCheckpointsW1.Add("EXTLVL1");
        defaultListForTokensOwned.Add(TokenManagerScript.DefaultToken);
        Load();
    }

    public void Save()
    {
        ES3.Save("W1Scenes", CheckPointManagerScript.W1Scenes);
        ES3.Save("TokensOwned", TokenManagerScript.TokensOwned);
        ES3.Save("SFXSoundVolume", SFXSoundSettings.value);
        ES3.Save("MusicSoundVolume", MusicSoundSettings.value);
    }

    public void Load()
    {
        CheckPointManagerScript.W1Scenes = ES3.Load("W1Scenes", defaultListForCheckpointsW1);
        TokenManagerScript.TokensOwned = ES3.Load("TokensOwned", TokenManagerScript.TokensOwned);
        TokenManagerScript.ActivateDefaultToken();
        SFXSoundSettings.value = ES3.Load("SFXSoundVolume", SFXSoundSettings.value);
        MusicSoundSettings.value = ES3.Load("MusicSoundVolume", MusicSoundSettings.value);

    }
    private void OnApplicationQuit()
    {
        Save();
    }
}
