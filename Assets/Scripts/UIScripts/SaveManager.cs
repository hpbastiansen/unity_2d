using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public CheckPointManager CheckPointManagerScript;
    public TokenManager TokenManagerScript;

    [Header("Default variables")]
    public List<string> defaultListForCheckpointsW1 = new List<string>();
    public List<GameObject> defaultListForTokensOwned;



    // Start is called before the first frame update
    void Start()
    {
        defaultListForCheckpointsW1.Add("LEVEL1_NOT");
        defaultListForTokensOwned.Add(TokenManagerScript.DefaultToken);


        Load();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Save();
        }
    }

    public void Save()
    {
        ES3.Save("W1Scenes", CheckPointManagerScript.W1Scenes);
        ES3.Save("TokensOwned", TokenManagerScript.TokensOwned);
    }

    public void Load()
    {
        CheckPointManagerScript.W1Scenes = ES3.Load("W1Scenes", defaultListForCheckpointsW1);
        TokenManagerScript.TokensOwned = ES3.Load("TokensOwned", defaultListForTokensOwned);
    }
}
