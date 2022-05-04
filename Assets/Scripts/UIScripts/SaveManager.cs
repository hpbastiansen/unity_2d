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
    public WeaponController WeaponControllerScript;


    [Header("Default variables")]
    public List<string> defaultListForCheckpointsW1 = new List<string>();
    public List<GameObject> defaultListForTokensOwned;



    // Start is called before the first frame update
    void Awake()
    {
        defaultListForCheckpointsW1.Add("STAGE1");
        defaultListForTokensOwned.Add(TokenManagerScript.DefaultToken);
        Load();
    }

    public void Save()
    {
        ES3.Save("W1Scenes", CheckPointManagerScript.W1Scenes);
        ES3.Save("TokensOwned", TokenManagerScript.TokensOwned);
        ES3.Save("SFXSoundVolume", SFXSoundSettings.value);
        ES3.Save("MusicSoundVolume", MusicSoundSettings.value);
        ES3.Save("Tutorial", CheckPointManagerScript.IsTutorialDone);
        ES3.Save("GrapplingHook", WeaponControllerScript.GottenGrapplingHook);
        ES3.Save("CactiInt", TokenManagerScript.CactiDestoyed);

    }

    public void Load()
    {
        CheckPointManagerScript.W1Scenes = ES3.Load("W1Scenes", defaultListForCheckpointsW1);
        TokenManagerScript.TokensOwned = ES3.Load("TokensOwned", TokenManagerScript.TokensOwned);
        TokenManagerScript.ActivateDefaultToken();
        SFXSoundSettings.value = ES3.Load("SFXSoundVolume", 0.5f);
        MusicSoundSettings.value = ES3.Load("MusicSoundVolume", 0.5f);
        CheckPointManagerScript.IsTutorialDone = ES3.Load("Tutorial", false);
        WeaponControllerScript.GottenGrapplingHook = ES3.Load("GrapplingHook", false);
        TokenManagerScript.CactiDestoyed = ES3.Load("CactiInt", 0);
    }
    private void OnApplicationQuit()
    {
        Save();
    }
}
