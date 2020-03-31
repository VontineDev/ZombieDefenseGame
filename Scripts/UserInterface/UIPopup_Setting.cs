using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_Setting : MonoBehaviour
{
    public UIRoot uIRoot;
    public UIButton btnClose;
    //public AudioSource bgmAudioSource;
    //public AudioSource effectAudioSource;
    public UISprite BgMusicOn;
    public UISprite BgMusicOff;
    public UISprite SoundEffectOn;
    public UISprite SoundEffectOff;

    public UIButton btnBgMusicOn;
    public UIButton btnBgMusicOff;
    public UIButton btnSoundEffectOn;
    public UIButton btnSoundEffectOff;

    public UIButton btnCredit;
    public UIPopup_Credit uIPopup_Credit;
    public UIButton btnCloseCrdit;
    public UIButton btnGPGS;

    public void Init(UIRoot uIRoot)
    {
        this.uIRoot = uIRoot;
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

    public void Start()
    {
        btnClose.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            this.gameObject.SetActive(false);
        }));
        btnBgMusicOn.onClick.Add(new EventDelegate(() =>
        {

            BgMusicOn.gameObject.SetActive(true);
            BgMusicOff.gameObject.SetActive(false);
            IsBgmOnCheck(true);
            SoundEffectManager.effectSoundAction();
            SoundBgManager.instance.MuteAudio(false); // 로비 배경음악 켬
        }));
        btnBgMusicOff.onClick.Add(new EventDelegate(() =>
        {

            BgMusicOn.gameObject.SetActive(false);
            BgMusicOff.gameObject.SetActive(true);
            IsBgmOnCheck(false);
            SoundEffectManager.effectSoundAction();
            SoundBgManager.instance.MuteAudio(true); // 로비 배경음악 끔
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

        btnCredit.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            if (this.uIPopup_Credit != null)
            {
                this.uIPopup_Credit.Open();
            }
            else
            {
                var prefab = Resources.Load<GameObject>("Prefab/UI/UIPopup_Credit");
                this.uIPopup_Credit = Instantiate(prefab).GetComponent<UIPopup_Credit>();
                this.uIPopup_Credit.transform.SetParent(uIRoot.transform);
                this.uIPopup_Credit.transform.localScale = Vector3.one;
                this.uIPopup_Credit.transform.localPosition = Vector3.zero;
                this.uIPopup_Credit.Init();
            }
            Debug.Log("Credit 작성");
        }));
        btnGPGS.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            GPGSManager.instance.Init();
            App.instance.gpgsLogin.CheckVersionOfGPGS(); // gpgs버전체크
            App.instance.gpgsLogin.GpgsSignIn();
        }));
    }

    public void Open()
    {
        this.gameObject.SetActive(true);

    }

    public void Close()
    {
        this.gameObject.SetActive(false);
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
