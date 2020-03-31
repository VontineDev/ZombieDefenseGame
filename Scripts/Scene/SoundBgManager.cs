using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBgManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip AnimAudioClip;
    public static SoundBgManager instance;


    // Start is called before the first frame update
    void Awake()
    {
        if (SoundBgManager.instance == null)
        {
            SoundBgManager.instance = this;
        }
    }
    public void PlayAnimAudio()
    {
        audioSource.clip = AnimAudioClip;
        Debug.Log("@@@@@@@@@@@@@@2음악실행@@@@@@@@@@@@@");
        audioSource.volume = 0.5f;
        audioSource.Play();
    }
    public void PlayBasicAudio(int count)
    {

    }
    public void StopAnimAudio()
    {
        audioSource.Stop();
    }
    public void MuteAudio(bool check)
    {
        audioSource.mute = check;
    }
}
