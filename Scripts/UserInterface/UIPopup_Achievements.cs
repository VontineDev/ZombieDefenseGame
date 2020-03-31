using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIPopup_Achievements : MonoBehaviour
{
    public UIButton btnClose;
    public UIScrollView scrollView;
    public UIGrid grid;
    public GameObject listItemPrefab;
    public UIListItemAchievement[] arrUIListItemAchievements;
    public GameObject uIPopupRewardClaimedNotice;

    public Action OnChangeMoney;
    public void Start()
    {
        btnClose.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            this.gameObject.SetActive(false);
        }));
    }

    public void Init()
    {
        DataManager.GetInstance().SetMonsterKillCountAchivementCleared();
        var dic = DataManager.GetInstance().dicAchievementsData;
        var info = DataManager.GetInstance().dicAchievementsInfo;
        arrUIListItemAchievements = new UIListItemAchievement[dic.Count];
        int i = 0;
        foreach (var keyVal in dic)
        {
            var listItem = Instantiate<GameObject>(listItemPrefab).GetComponent<UIListItemAchievement>();
            listItem.Init(keyVal.Value, info[keyVal.Key]);
            listItem.transform.parent = grid.transform;
            listItem.transform.localScale = Vector3.one;
            listItem.OnBtnClaimClicked = () =>
            {
                PopupRewardClaimedNotice();
                OnChangeMoney();
            };
            arrUIListItemAchievements[i] = listItem;
            i++;
        }
        this.grid.Reposition();
        this.scrollView.ResetPosition();
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        ResetListItemData();
        this.grid.Reposition();
        this.scrollView.ResetPosition();
    }
    private void ResetListItemData()
    {
        DataManager.GetInstance().SetMonsterKillCountAchivementCleared();
        var dic = DataManager.GetInstance().dicAchievementsData;
        var info = DataManager.GetInstance().dicAchievementsInfo;
        int i = 0;
        foreach (var keyVal in dic)
        {
            arrUIListItemAchievements[i].Init(keyVal.Value, info[keyVal.Key]);
            i++;
        }
    }
    public void Close()
    {
        this.gameObject.SetActive(false);
    }
    public void PopupRewardClaimedNotice()
    {
        StartCoroutine(PopupRewardClaimedNoticeImpl());
    }
    IEnumerator PopupRewardClaimedNoticeImpl()
    {
        uIPopupRewardClaimedNotice.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(2f);
            uIPopupRewardClaimedNotice.SetActive(false);
            break;
        }
    }
}
