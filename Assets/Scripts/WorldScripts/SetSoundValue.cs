using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSoundValue : MonoBehaviour
{
    [HideInInspector] public AudioSource _audioSource;
    [HideInInspector] public SoundManager _soundManager;
    public float MaxVolume;
    public bool SFX;
    public bool Music;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _soundManager = Object.FindObjectOfType<SoundManager>();
        MaxVolume = _audioSource.volume * 2;
    }
    // Start is called before the first frame update
    void Start()
    {

        if (Music) _audioSource.volume = MaxVolume * _soundManager.MusicValue;
        if (SFX) _audioSource.volume = MaxVolume * _soundManager.SFXValue;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeSoundValue()
    {
        if (Music) _audioSource.volume = MaxVolume * _soundManager.MusicValue;
        if (SFX) _audioSource.volume = MaxVolume * _soundManager.SFXValue;
    }
}
