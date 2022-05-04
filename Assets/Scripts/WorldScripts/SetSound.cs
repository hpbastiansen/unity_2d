using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSound : MonoBehaviour
{
    public bool IsDash;
    public bool IsGun;
    public bool IsGrapplinghook;


    private AudioSource _audioSource;
    private SoundManager _soundManager;
    private TokenManager _tokenManager;
    private WeaponController _weaponController;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _soundManager = Object.FindObjectOfType<SoundManager>();
        _tokenManager = Object.FindObjectOfType<TokenManager>();
        _weaponController = Object.FindObjectOfType<WeaponController>();

    }
    void Update()
    {
        if (IsDash)
        {
            if (_tokenManager.DefaultTokenActive) { _audioSource.clip = _soundManager.DashNormal; }
            if (_tokenManager.CactusTokenActive) { _audioSource.clip = _soundManager.DashNormal; }
            if (_tokenManager.RevloverTokenActive) { _audioSource.clip = _soundManager.DashNormal; }
            if (_tokenManager.WormTokenActive) { _audioSource.clip = _soundManager.DashWorm; }
        }
        if (IsGrapplinghook)
        {
            _audioSource.clip = _soundManager.Grapplinghook;
        }
    }
}
