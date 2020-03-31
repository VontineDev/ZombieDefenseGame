using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPopup_Fail : MonoBehaviour
{   
    public int comfortCash;
    public UIButton btnLobby;
    public System.Action OnBtnLobbyClicked;
    public UILabel Lb_comfortCash;
    public UIButton btnReStart;
    void Start()
    {
        this.btnLobby.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            OnBtnLobbyClicked();
            GetReward();
        }));
        this.btnReStart.onClick.Add(new EventDelegate(() => {
            SoundEffectManager.effectSoundAction();
            GetReward();
            SceneManager.LoadScene("DefenseMode");
        }));
    }
    public void GetReward()
    {
        DataManager.GetInstance().userInfo.cash += (ulong)comfortCash;
        Debug.Log($"보상을 {comfortCash} 받았습니다.");
        DataManager.GetInstance().SaveUserInfo();
    }

    public void Init(int comfortCash)
    {
        this.comfortCash = comfortCash;
        Lb_comfortCash.text = this.comfortCash.ToString();
    }
}
