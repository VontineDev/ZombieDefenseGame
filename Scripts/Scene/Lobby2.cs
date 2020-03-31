using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class Lobby2 : MonoBehaviour
{
    public UIButton btnIdle02; //화면 전체 버튼, 클릭하면 지휘관 애니메이션 실행

    //로비에서 지휘관 위치
    public Vector3 commanderRotation;
    public Vector3 commaderPosition;
    public int presentComm_id;
    public GameObject commanderGo;


    public UI_LobbyButtons uI_LobbyButtons; //버튼들을 관리하는 스크립트
    public UIPanel_Money uIPanel_Money; //재화 표시 패널
    public UIRoot uiRoot;
    private UIPopup_Setting uiPopupSetting; //옵션
    private GameObject uiPopupSettingPrefab; //옵션프리팹
    private UIPopup_Achievements uiPopupAchievement; //업적
    private GameObject uiPopupAchievementPrefab; //업적프리팹

    private Coroutine coLobbyIdle;
    void Start()
    {

        SoundInGameBgManager.instance.StopAnimAudio();
        SoundBgManager.instance.PlayAnimAudio();

        if (DataManager.GetInstance().userInfo.isBgmOn == true)
        {
            SoundBgManager.instance.MuteAudio(false);
        }
        else
        {
            SoundBgManager.instance.MuteAudio(true);
        }

        commanderRotation = new Vector3(0, 180, 0);
        commaderPosition = new Vector3(0, 0, -3f);
        SetTimeScale(1f);

        var commander_Id = GetPresentCommander_id();
        SetCommander(commander_Id);

        //load ui popup
        this.uiPopupSettingPrefab = Resources.Load<GameObject>("Prefab/UI/UIPopup_Setting");
        Debug.LogFormat("uiPopupSettingPrefab: ", uiPopupSettingPrefab);

        this.uiPopupAchievementPrefab = Resources.Load<GameObject>("Prefab/UI/UIPopup_Achievements");
        Debug.LogFormat("uiPopupAchievementPrefab: ", uiPopupAchievementPrefab);

        this.AddEventsLobbyButtons();

        uI_LobbyButtons.Init();
        uI_LobbyButtons.OnSetCommObjCallback = (commander_id) =>
        {
            SetCommander(commander_id);
        };
        uIPanel_Money.Init();
        uIPanel_Money.OnMoneyChange(0);
        uI_LobbyButtons.uIPopup_MyInfo.onChangeMoney = () =>
        {
            uIPanel_Money.DisplayMoney();
        };

    }

    //로비 버튼에 이벤트 추가
    private void AddEventsLobbyButtons()
    {
        this.btnIdle02.onClick.Add(new EventDelegate(() =>
        {
            var anim = commanderGo.GetComponentInChildren<Animator>();
            anim.Play($"Lobby_Idle02");
            if (coLobbyIdle != null)
            {
                StopCoroutine(coLobbyIdle);
            }
            coLobbyIdle = StartCoroutine(PlayAnim_Idle01(anim));
        }));

        this.uI_LobbyButtons.btnAchievements.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            this.uI_LobbyButtons.CloseAllPopup();
            if (this.uiPopupAchievement != null)
            {
                this.uiPopupAchievement.Open();
            }
            else
            {
                this.uiPopupAchievement = Instantiate<GameObject>(this.uiPopupAchievementPrefab).GetComponent<UIPopup_Achievements>();

                this.uiPopupAchievement.transform.SetParent(this.uiRoot.transform);
                this.uiPopupAchievement.transform.localScale = Vector3.one;
                this.uiPopupAchievement.transform.localPosition = Vector3.zero;
                this.uiPopupAchievement.Init();
                this.uiPopupAchievement.OnChangeMoney = () =>
                {
                    uIPanel_Money.DisplayMoney();
                };
            }
        }));

        this.uI_LobbyButtons.btnSetting.onClick.Add(new EventDelegate(() =>
        {

            SoundEffectManager.effectSoundAction();
            this.uI_LobbyButtons.CloseAllPopup();

            if (this.uiPopupSetting != null)
            {
                this.uiPopupSetting.Init(uiRoot);
                this.uiPopupSetting.Open();

            }
            else
            {
                this.uiPopupSetting = Instantiate<GameObject>(this.uiPopupSettingPrefab).GetComponent<UIPopup_Setting>();
                uiPopupSetting.Init(uiRoot);
                Debug.LogFormat("uiPopupSetting: {0} uiRoot: {1}", uiPopupSetting, uiRoot);

                this.uiPopupSetting.transform.SetParent(this.uiRoot.transform);
                this.uiPopupSetting.transform.localScale = Vector3.one;
                this.uiPopupSetting.transform.localPosition = Vector3.zero;
            }

        }));
    }

    private void SetTimeScale(float num)
    {
        Time.timeScale = num;
    }

    //현재 선택된 커맨더 아이디 받기
    private int GetPresentCommander_id()
    {
        var selectedData = (from info in DataManager.GetInstance().dicCommanderinfo
                            where info.Value.isSelected == true
                            select info.Value).FirstOrDefault();
        return selectedData.commander_id;
    }
    //선택된 커맨더를 로비화면에 띄움
    private void SetCommander(int commander_id)
    {
        if (commanderGo != null)
        {
            Destroy(commanderGo);
        }

        var data = DataManager.GetInstance().dicCommanderData[commander_id];
        UnityEngine.Object prefab = (from obj in App.instance.resource
                                     where obj.name == data.prefab_name
                                     select obj).FirstOrDefault();
        commanderGo = (GameObject)Instantiate(prefab);
        commanderGo.transform.position = commaderPosition;
        commanderGo.transform.rotation = Quaternion.Euler(0, 180, 0);

        var anim = commanderGo.GetComponentInChildren<Animator>();
        anim.Play($"Lobby_Idle01");
        SetCommanderInfo(commander_id);
    }
    IEnumerator PlayAnim_Idle01(Animator animator)
    {
        animator.Play("LobbyIdle02");
        yield return new WaitForSeconds(3.2f);
        animator.Play("Lobby_Idle01");
    }
    private void SetCommanderInfo(int commander_id)
    {
        var commInfo = DataManager.GetInstance().dicCommanderinfo;
        foreach (var info in commInfo)
        {
            info.Value.isSelected = false;
        }
        commInfo[commander_id].isSelected = true;
    }

}
