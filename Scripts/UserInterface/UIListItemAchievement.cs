using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIListItemAchievement : MonoBehaviour
{
    public UILabel lbName;
    public UILabel lbDesc;
    public UILabel lbRwd;
    public UIButton btnClaim;
    public UISprite iconClaimed;
    public UISprite iconNotCleared;
    public AchievementsData data;
    public AchievementsInfo info;
    public Action OnBtnClaimClicked;

    public void Init(AchievementsData data, AchievementsInfo info)
    {
        this.data = data;
        this.lbName.text = data.name;
        this.lbDesc.text = data.description;
        this.lbRwd.text = data.amount.ToString();
        this.info = info;
        if (info.isCleared == true && info.isClaimed == false)
        {
            this.btnClaim.gameObject.SetActive(true);
        }
        else if (info.isCleared == true && info.isClaimed == true)
        {
            this.iconClaimed.gameObject.SetActive(true);
        }
        else if (info.isCleared == false && info.isClaimed == false)
        {
            this.iconNotCleared.gameObject.SetActive(true);
        }
        else
        {
            this.btnClaim.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        this.btnClaim.onClick.Add(new EventDelegate(() =>
       {
           Debug.Log($"{name} 보상을 받았습니다");
           DataManager.GetInstance().AddCash((ulong)data.amount);
           DataManager.GetInstance().SaveUserInfo();
           this.info.isClaimed = true;
           this.btnClaim.gameObject.SetActive(false);
           OnBtnClaimClicked();
       }));
    }
}
