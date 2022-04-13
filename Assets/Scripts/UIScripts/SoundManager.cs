using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
