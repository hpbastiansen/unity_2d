using UnityEngine;

/// This script sets the correct sounds on dash and grappling hook for the different tokens equipped.
public class SetSound : MonoBehaviour
{
    public bool IsDash;
    public bool IsGun;
    public bool IsGrapplinghook;

    private AudioSource _audioSource;
    private SoundManager _soundManager;
    private TokenManager _tokenManager;

    /// Called before the first frame.
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _soundManager = FindObjectOfType<SoundManager>();
        _tokenManager = FindObjectOfType<TokenManager>();
    }

    /// Called every frame.
    /** Check which token is active and if the player is currently grappling and set the corresponding sound. */
    void Update()
    {
        if (IsDash)
        {
            if (_tokenManager.DefaultTokenActive) { _audioSource.clip = _soundManager.DashNormal; }
            if (_tokenManager.CactusTokenActive) { _audioSource.clip = _soundManager.DashNormal; }
            if (_tokenManager.RevolverTokenActive) { _audioSource.clip = _soundManager.DashNormal; }
            if (_tokenManager.WormTokenActive) { _audioSource.clip = _soundManager.DashWorm; }
        }
        if (IsGrapplinghook)
        {
            _audioSource.clip = _soundManager.Grapplinghook;
        }
    }
}
