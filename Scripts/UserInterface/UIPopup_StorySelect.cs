using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_StorySelect : MonoBehaviour
{
    public ListItem_Stage[] arrListItem_Stages;
    public UIButton btnClose;

    void Start()
    {
        this.btnClose.onClick.Add(new EventDelegate(() =>
        {
            this.gameObject.SetActive(false);
        }));
        Init();
    }
    public void Init()
    {

        for (int i = 0; i < arrListItem_Stages.Length; i++)
        {
            var capt = i;

            arrListItem_Stages[capt].OnBtnClicked = (stageState) =>
        {
            if (stageState != eStageState.Locked)
            {
                App.instance.stageNumber = capt + 1;
                SceneLoader.GetInstance().LoadSingleScene("DefenseMode");
            }
        };
        }
    }

    public void Open()
    {
        Debug.Log("----------------------------Story Select OPEN");

        for (int i = 0; i < arrListItem_Stages.Length; i++)
        {
            var capt = i;
            var stageClearInfo = DataManager.GetInstance().dicStageClearInfo[capt + 1];
            arrListItem_Stages[capt].Init(stageClearInfo);
        }
    }

}
