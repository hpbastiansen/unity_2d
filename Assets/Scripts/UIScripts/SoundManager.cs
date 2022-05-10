using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// This script is responsible for managing background music and SFX volume.
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
    public SetSoundValue[] _setSoundValue;

    /// Called at initialization, before all objects Start() methods.
    /** Set music and SFX volume values. */
    private void Awake()
    {
        MusicValue = MusicSlider.value;
        SFXValue = SFXSlider.value;
        _setSoundValue = FindObjectsOfType<SetSoundValue>();
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    /// Called before the first frame.
    /** Play a random background music track on startup. */
    private void Start()
    {
        int _randomMusic = Random.Range(0, BackgroundMusicList.Count);
        BackgroundMusicSource.clip = BackgroundMusicList[_randomMusic];
        BackgroundMusicSource.Play();

    }

    /// Called every frame.
    /** If the background music track is finished, play a new track. */
    void Update()
    {
        if (BackgroundMusicSource.isPlaying == false)
        {
            int _randomMusic = Random.Range(0, BackgroundMusicList.Count);
            BackgroundMusicSource.clip = BackgroundMusicList[_randomMusic];
            BackgroundMusicSource.Play();
        }

    }

    /// Set the music and SFX volume to what is selected in the UI.
    public void ChangeValue()
    {
        MusicValue = MusicSlider.value;
        MusicText.text = (MusicValue * 100).ToString("0") + "%";
        SFXValue = SFXSlider.value;
        SFXText.text = (SFXValue * 100).ToString("0") + "%";

        foreach (var sound in _setSoundValue)
        {
            if (sound.Music) sound._audioSource.volume = sound.MaxVolume * MusicValue;
            if (sound.SFX) sound._audioSource.volume = sound.MaxVolume * SFXValue;
        }
    }

    /// Set the background music pitch on scene load. If it's the boss stage, pitch music up, otherwise pitch down.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "STAGE5")
        {
            BackgroundMusicSource.pitch = 1.2f;
        }
        else
        {
            BackgroundMusicSource.pitch = 0.6f;
        }
    }
}