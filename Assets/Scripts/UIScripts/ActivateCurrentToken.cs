using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCurrentToken : MonoBehaviour
{
    private TokenManager _tokenManager;
    public string TokenActivate;
    private void Awake()
    {
        _tokenManager = Object.FindObjectOfType<TokenManager>();
    }
    private void OnEnable()
    {
        _tokenManager.Invoke(TokenActivate, 0f);
    }
}
