using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPopup_Victory : MonoBehaviour
{
    public int rewardCash;
    public UIButton btnLobby;
    public UIButton btnRestart;
    public UIButton btnNextStage;
    public System.Action OnBtnLobbyClicked;
    public UILabel Lb_rewardCash;
    void Start()
    {
        this.btnLobby.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            OnBtnLobbyClicked();
            GetReward();
        }));
        this.btnNextStage.onClick.Add(new EventDelegate(() => {
            SoundEffectManager.effectSoundAction();
            if (App.instance.stageNumber < 16)
            {
                App.instance.stageNumber += 1;
            }
            GetReward();
            SceneManager.LoadScene("DefenseMode");
        }));
        this.btnRestart.onClick.Add(new EventDelegate(() => {
            SoundEffectManager.effectSoundAction();
            GetReward();
            SceneManager.LoadScene("DefenseMode");
        }));
    }
    public void GetReward()
    {
        DataManager.GetInstance().userInfo.cash += (ulong)rewardCash;
        Debug.Log($"보상을 {rewardCash} 받았습니다.");
        DataManager.GetInstance().SaveUserInfo();
    }

    public void Init(int rewardCash)
    {
        this.rewardCash = rewardCash;
        Lb_rewardCash.text = this.rewardCash.ToString();
    }
}
