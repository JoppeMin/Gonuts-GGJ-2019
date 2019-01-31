using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioClip SnowWalk, SummerWalk,
        FallAmbient, WinterAmbient, Backgroundmusic,
        Squirrel,
        Digging, FindNut, Death,
        WinMusic, LoseMusic;
    public static AudioSource audioEventSrc, audioAmbientSrc, audioMusicSrc;
    // Start is called before the first frame update
    void Start()
    {
        SummerWalk = Resources.Load<AudioClip>("SummerWalk");
        SnowWalk = Resources.Load<AudioClip>("SnowWalk 1");
        FallAmbient = Resources.Load<AudioClip>("ForestAmbience02");
        WinterAmbient = Resources.Load<AudioClip>("SnowAmbience01");
        Squirrel = Resources.Load<AudioClip>("Squirrel01");
        Digging = Resources.Load<AudioClip>("Digging");
        Death = Resources.Load<AudioClip>("Death");
        WinMusic = Resources.Load<AudioClip>("WinMusic");
        LoseMusic = Resources.Load<AudioClip>("LoseMusic");
        FindNut = Resources.Load<AudioClip>("FindNut");
        Backgroundmusic = Resources.Load<AudioClip>("BackgroundMusic");

        audioEventSrc = GetComponent<AudioSource>();
        audioAmbientSrc = GameObject.Find("AmbientAudioSource").GetComponent<AudioSource>();
        audioMusicSrc = GameObject.Find("MusicAudioSource").GetComponent<AudioSource>();
        PlayAmbient("Fall");
        PlayMusic("BackGround");
    }

    public static void PlayMusic(string clip)
    {
        switch (clip)
        {
            case "BackGround":
                audioMusicSrc.loop = true;
                audioMusicSrc.clip = Backgroundmusic;
                audioMusicSrc.Play();
                break;

            case "WinMusic":
                audioMusicSrc.loop = false;
                audioMusicSrc.clip = WinMusic;
                audioMusicSrc.Play();
                break;

            case "LoseMusic":
                audioMusicSrc.loop = false;
                audioMusicSrc.clip = LoseMusic;
                audioMusicSrc.Play();
                break;

            default:
                audioMusicSrc.loop = true;
                audioMusicSrc.clip = Backgroundmusic;
                audioMusicSrc.Play();
                break;

        }
    }

    public static void PlayAmbient(string clip)
    {
        switch (clip)
        {
            case "Fall":
                audioAmbientSrc.clip = FallAmbient;
                audioAmbientSrc.Play();
                break;

            case "Winter":
                audioAmbientSrc.clip = WinterAmbient;
                audioAmbientSrc.Play();
                break;
        }
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "Walk":
                float rnd = Random.Range(0.9f, 1.1f);
                audioEventSrc.pitch = rnd;
                if (GameManager.instance.currentState == GameManager.GameState.TutorialFall || GameManager.instance.currentState == GameManager.GameState.PlayingFall)
                {
                    audioEventSrc.PlayOneShot(SummerWalk);
                }
                else if (GameManager.instance.currentState == GameManager.GameState.TutorialWinter || GameManager.instance.currentState == GameManager.GameState.PlayingWinter)
                {
                    audioEventSrc.PlayOneShot(SnowWalk);
                }
                    break;

            case "Squirrel":
                audioEventSrc.PlayOneShot(Squirrel);
                break;

            case "Digging":
                audioEventSrc.PlayOneShot(Digging);
                break;
            case "FindNut":
                audioAmbientSrc.PlayOneShot(FindNut);
                break;
            case "Death":
                audioEventSrc.PlayOneShot(Death);
                break;
        }
        
    }
}
