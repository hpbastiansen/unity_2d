using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCurrentToken : MonoBehaviour
{
    private TokenManager TokenManagerScript;
    public string TokenActivate;
    private void Awake()
    {
        TokenManagerScript = Object.FindObjectOfType<TokenManager>();
    }
    private void OnEnable()
    {
        TokenManagerScript.Invoke(TokenActivate, 0f);
    }
}
