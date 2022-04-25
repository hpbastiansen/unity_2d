using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Weapons")]
    public AudioClip DefaultGun;
    public AudioClip Grapplinghook;

    [Header("Dashes")]
    public AudioClip DashNormal;
    public AudioClip DashWorm;

    [Header("Hit effects")]
    public AudioClip HitWall;

    /*[Header("Movement")]*/

    [Header("Music")]
    public List<AudioClip> BackgroundMusicList;
    public AudioSource BackgroundMusicSource;

    [Header("Settings")]
    public Slider SFXSlider;
    public Text SFXText;
    public float SFXValue;
    public Slider MusicSlider;
    public Text MusicText;
    public float MusicValue;

    private void Awake()
    {
        MusicValue = MusicSlider.value;
        SFXValue = SFXSlider.value;
    }
    private void Start()
    {
        int _randomMusic = Random.Range(0, BackgroundMusicList.Count);
        BackgroundMusicSource.clip = BackgroundMusicList[_randomMusic];
        BackgroundMusicSource.Play();

    }

    void Update()
    {
        if (BackgroundMusicSource.isPlaying == false)
        {
            int _randomMusic = Random.Range(0, BackgroundMusicList.Count);
            BackgroundMusicSource.clip = BackgroundMusicList[_randomMusic];
            BackgroundMusicSource.Play();
        }
    }
    public void ChangeValue()
    {
        MusicValue = MusicSlider.value;
        MusicText.text = (MusicValue * 100).ToString("0") + "%";
        SFXValue = SFXSlider.value;
        SFXText.text = (SFXValue * 100).ToString("0") + "%";

        SetSoundValue[] _setSoundValue = Object.FindObjectsOfType<SetSoundValue>();
        foreach (var sound in _setSoundValue)
        {
            if (sound.Music) sound._audioSource.volume = sound.MaxVolume * MusicValue;
            if (sound.SFX) sound._audioSource.volume = sound.MaxVolume * SFXValue;
        }
    }
}
