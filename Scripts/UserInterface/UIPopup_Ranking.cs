using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_Ranking : MonoBehaviour
{

    public void Start()
    {

    }

    public void OpenLeaderBoard()
    {
        if (Social.localUser.id != null)
        {
            var score = DataManager.GetInstance().userInfo.monsterKillCount;
            GPGSManager.instance.AddLeaderboard(score);
            GPGSManager.instance.ShowLeaderboardUI();
        }
        else
        {
            GPGSManager.instance.Init();
            App.instance.gpgsLogin.GpgsSignIn();

            var score = DataManager.GetInstance().userInfo.monsterKillCount;
            GPGSManager.instance.AddLeaderboard(score);
            GPGSManager.instance.ShowLeaderboardUI();
        }
    }
}
