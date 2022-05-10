using UnityEngine;

/// This script sets the sound value of music and SFX when the method is called.
public class SetSoundValue : MonoBehaviour
{
    public AudioSource _audioSource;
    [HideInInspector] public SoundManager _soundManager;
    public float MaxVolume;
    public bool SFX;
    public bool Music;

    /// Called at initialization, before all objects Start() methods.
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _soundManager = FindObjectOfType<SoundManager>();
        MaxVolume = _audioSource.volume * 2;
    }

    /// Called before the first frame.
    void Start()
    {
        ChangeSoundValue();
    }

    /// Set the volume of Music and SFX in relation to the max volume.
    public void ChangeSoundValue()
    {
        if (Music) _audioSource.volume = MaxVolume * _soundManager.MusicValue;
        if (SFX) _audioSource.volume = MaxVolume * _soundManager.SFXValue;
    }
}
