using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveTokens : MonoBehaviour
{
    public TokenManager TokenManagerScript;
    void Start()
    {
        TokenManagerScript = Object.FindObjectOfType<TokenManager>();
    }
    public void GiveCactusToken()
    {
        TokenManagerScript.AddTokens(TokenManagerScript.CactusToken);
    }
    public void GiveRevloverToken()
    {
        TokenManagerScript.AddTokens(TokenManagerScript.RevloverToken);
    }
    public void GiveWormToken()
    {
        TokenManagerScript.AddTokens(TokenManagerScript.WormToken);
    }
}
