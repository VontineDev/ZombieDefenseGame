using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] AnimAudioClip;
    public static SoundManager instance;


    // Start is called before the first frame update
    void Awake()
    {        
        if (SoundManager.instance == null)
        {
            SoundManager.instance = this;
        }
    }
    public void PlayAnimAudio(int count)
    {
        audioSource.volume = 0.5f;
        audioSource.PlayOneShot(AnimAudioClip[count]);
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
        audioSource.mute =  check;
    }
    
}
