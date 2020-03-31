using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundEffectManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] AnimAudioClip;
    public static SoundEffectManager instance;
    public static Action effectSoundAction;
    public static Action effectUpgradeAction;

    // Start is called before the first frame update
    void Awake()
    {
        if (SoundEffectManager.instance == null)
        {
            SoundEffectManager.instance = this;
        }
        effectSoundAction = () => {
            SoundEffectManager.instance.PlayAnimAudio(5);
        };
        effectUpgradeAction = () =>
        {
            SoundEffectManager.instance.PlayAnimAudio(6);
        };
    }
    public void PlayAnimAudio(int count)
    {
        Debug.Log("@@@@@@@@@@@@@@2음악실행@@@@@@@@@@@@@");
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
        audioSource.mute = check;
    }
}
