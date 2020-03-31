using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class UI_LobbyButtons : MonoBehaviour
{
    //지휘관선택 좌우버튼
    public UIButton btnPrevCommander;
    public UIButton btnNextCommander;
    //설정
    public UIButton btnSetting;

    //public UIPopup_Setting uIPopup_Setting;
    //인벤토리
    public UIButton btnInventory;

    public UIPopup_Inventory uIPopup_Inventory;
    //내정보
    public UIButton btnMyInfo;

    public UIPopup_MyInfo uIPopup_MyInfo;
    //업적
    public UIButton btnAchievements;

    public UIPopup_Achievements uIPopup_Achievements;
    //랭킹
    public UIButton btnRangking;

    public UIPopup_Ranking uIPopup_Ranking;
    //게임시작
    public UIButton btnStart;
    public UIPopup_StorySelect uIPopup_StorySelect;
    //현재 선택된 지휘관
    public int presentCommander_id;
    public Action<int> OnSetCommObjCallback;
    private void Start()
    {
        SetBtnSelectComm();
        SetBtnInventory();
        SetBtnMyInfo();
        //SetBtnAchievements();
        SetBtnRanking();
        SetBtnStart();
    }

    public void Init()
    {
        GetPresentCommander_id();

        uIPopup_MyInfo.Init();
    }
    private void GetPresentCommander_id()
    {
        var selectedData = (from info in DataManager.GetInstance().dicCommanderinfo
                            where info.Value.isSelected == true
                            select info.Value).FirstOrDefault();
        this.presentCommander_id = selectedData.commander_id;
    }
    private void SetBtnSelectComm()
    {
        btnPrevCommander.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            this.presentCommander_id -= 1;
            if (presentCommander_id < 1)
            {
                presentCommander_id = 5;
            }
            Debug.Log($"presentCommander_id:{presentCommander_id}");

            OnSetCommObjCallback(presentCommander_id);
        }));

        btnNextCommander.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            this.presentCommander_id += 1;
            if (presentCommander_id > 5)
            {
                presentCommander_id = 1;
            }
            Debug.Log($"presentCommander_id:{presentCommander_id}");
            OnSetCommObjCallback(presentCommander_id);
        }));
    }
    private void SetBtnInventory()
    {
        btnInventory.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            CloseAllPopup();
            uIPopup_Inventory.gameObject.SetActive(true);
            uIPopup_Inventory.Init();
        }));

    }
    private void SetBtnMyInfo()
    {
        btnMyInfo.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            CloseAllPopup();
            uIPopup_MyInfo.gameObject.SetActive(true);
            uIPopup_MyInfo.DisplayInfos();
        }));

    }
    //private void SetBtnAchievements()
    //{
    //    btnAchievements.onClick.Add(new EventDelegate(() =>
    //    {
    //        CloseAllPopup();
    //        uIPopup_Achievements.gameObject.SetActive(true);
    //    }));
    //}
    private void SetBtnRanking()
    {
        btnRangking.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            uIPopup_Ranking.OpenLeaderBoard();
        }));
    }

    private void SetBtnStart()
    {
        btnStart.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            CloseAllPopup();
            uIPopup_StorySelect.gameObject.SetActive(true);
            Debug.Log("StartBTN");

            uIPopup_StorySelect.Open();
        }));
    }
    public void CloseAllPopup()
    {
        //uIPopup_Setting.transform.gameObject.SetActive(false);
        uIPopup_Inventory.transform.gameObject.SetActive(false);
        uIPopup_MyInfo.transform.gameObject.SetActive(false);
        //uIPopup_Achievements.transform.gameObject.SetActive(false);
        uIPopup_Ranking.transform.gameObject.SetActive(false);
    }
}
