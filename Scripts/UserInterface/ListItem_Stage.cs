using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum eStageState { Locked, Unlocked, Cleared }
public class ListItem_Stage : MonoBehaviour
{
    public UISprite iconLocked;
    public UISprite iconCleared;
    public UILabel lbStageNumber;
    public UIButton btnStageEnter;
    public StageClearInfo stageClearInfo;
    public System.Action<eStageState> OnBtnClicked;

    public eStageState stageState;

    private void Start()
    {
        this.btnStageEnter.onClick.Add(new EventDelegate(() =>
        {
            OnBtnClicked(stageState);
        }));
    }
    public void Init(StageClearInfo stageClearInfo)
    {
        this.stageClearInfo = stageClearInfo;
        SetStage();
    }

    public void SetStage()
    {
        this.lbStageNumber.text = stageClearInfo.id.ToString();
        if (this.stageClearInfo.isLocked == true)
        {
            SetStageLocked();
        }
        else
        {
            if (this.stageClearInfo.isCleared == true)
            {
                SetStageCleared();
            }
            else
            {
                SetStageUnlocked();
            }
        }
    }

    public void SetStageCleared()
    {
        this.iconLocked.gameObject.SetActive(false);
        this.iconCleared.gameObject.SetActive(true);
        stageState = eStageState.Cleared;
    }
    public void SetStageLocked()
    {
        this.iconLocked.gameObject.SetActive(true);
        stageState = eStageState.Locked;
    }
    public void SetStageUnlocked()
    {
        this.iconLocked.gameObject.SetActive(false);
        this.iconCleared.gameObject.SetActive(false);
        stageState = eStageState.Unlocked;
    }
}
