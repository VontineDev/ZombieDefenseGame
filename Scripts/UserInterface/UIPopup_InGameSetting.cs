using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_InGameSetting : MonoBehaviour
{
    public UIButton btnLobby;
    public UIButton btnRestart;
    public UIButton btnClose;

    public UISprite BgMusicOn;
    public UISprite BgMusicOff;
    public UISprite SoundEffectOn;
    public UISprite SoundEffectOff;

    public UIButton btnBgMusicOn;
    public UIButton btnBgMusicOff;
    public UIButton btnSoundEffectOn;
    public UIButton btnSoundEffectOff;
    private void Start()
    {

    }
    public void CheckMusic()
    {
        if (DataManager.GetInstance().userInfo.isBgmOn == true)
        {

            BgMusicOn.gameObject.SetActive(true);
            BgMusicOff.gameObject.SetActive(false);
        }
        if (DataManager.GetInstance().userInfo.isBgmOn == false)
        {

            BgMusicOn.gameObject.SetActive(false);
            BgMusicOff.gameObject.SetActive(true);
        }
        if (DataManager.GetInstance().userInfo.isFXsoundOn == true)
        {

            SoundEffectOn.gameObject.SetActive(true);
            SoundEffectOff.gameObject.SetActive(false);
        }
        if (DataManager.GetInstance().userInfo.isFXsoundOn == false)
        {

            SoundEffectOn.gameObject.SetActive(false);
            SoundEffectOff.gameObject.SetActive(true);
        }
    }
    public void Init()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        //Debug.Log("인게임세팅 스타트");

        this.btnClose.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            Debug.Log("닫기");

            SetTimeScale(1f);
            this.gameObject.SetActive(false);
        }));

        this.btnLobby.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            SceneLoader.GetInstance().LoadSingleScene("Lobby2");
        }));

        this.btnRestart.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            SceneLoader.GetInstance().LoadSingleScene("DefenseMode");
        }));

        btnBgMusicOn.onClick.Add(new EventDelegate(() =>
        {

            BgMusicOn.gameObject.SetActive(true);
            BgMusicOff.gameObject.SetActive(false);
            IsBgmOnCheck(true);
            SoundEffectManager.effectSoundAction();
            SoundInGameBgManager.instance.MuteAudio(false); // 인게임 배경음악 켬
        }));
        btnBgMusicOff.onClick.Add(new EventDelegate(() =>
        {

            BgMusicOn.gameObject.SetActive(false);
            BgMusicOff.gameObject.SetActive(true);
            IsBgmOnCheck(false);
            SoundEffectManager.effectSoundAction();
            SoundInGameBgManager.instance.MuteAudio(true); // 인게임 배경음악 끔
        }));
        btnSoundEffectOn.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectOn.gameObject.SetActive(true);
            SoundEffectOff.gameObject.SetActive(false);
            isFXsoundOnCheck(true);
            SoundEffectManager.effectSoundAction();
            SoundEffectManager.instance.MuteAudio(false); // 효과음 켬
        }));
        btnSoundEffectOff.onClick.Add(new EventDelegate(() =>
        {

            SoundEffectOn.gameObject.SetActive(false);
            SoundEffectOff.gameObject.SetActive(true);
            isFXsoundOnCheck(false);
            SoundEffectManager.effectSoundAction();
            SoundEffectManager.instance.MuteAudio(true); // 효과음 끔
        }));

        SetTimeScale(0);
    }
    private void SetTimeScale(float num)
    {
        Time.timeScale = num;
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }
    public void IsBgmOnCheck(bool check)
    {
        DataManager.GetInstance().userInfo.isBgmOn = check;
        DataManager.GetInstance().SaveUserInfo();
    }
    public void isFXsoundOnCheck(bool check)
    {
        DataManager.GetInstance().userInfo.isFXsoundOn = check;
        DataManager.GetInstance().SaveUserInfo();
    }
}
