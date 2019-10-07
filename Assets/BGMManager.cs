using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip bgm1;
    public AudioClip bgm2;

    public static BGMManager instance;

    private AudioSource source;

    public void Start()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlayBGM1()
    {
        StopBGM();
        source.clip = bgm1;
        source.Play();
    }

    public void PlayBGM2()
    {
        StopBGM();
        source.clip = bgm2;
        source.Play();
    }

    public void StopBGM()
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
    }
}
