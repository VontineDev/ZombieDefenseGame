using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using MarchingBytes;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class DefenseMode : MonoBehaviour
{
    public EasyObjectPool easyObjectPool;

    #region UI_FIELDS
    public Camera mainCam;
    public UIRoot uiRoot;
    public UI_UserInteraction uI_UserInteraction; 
    public UI_FenceHp uI_FenceHp;
    public UIPopup_InGameSetting uiPopup_InGameSetting;
    public UIPopup_Victory uIPopup_Victory; //승리팝업
    public UIPopup_Fail uIPopup_Fail; //패배팝업
    public UILabel lbMonsterCount; //남은몬스터표시 라벨
    private int monsterCount; //남은몬스터
    public UIButton btnStart; //테스트용 시작버튼
    public UIButton btnNormalSpeed; //TIMESCALE 조절 버튼 1X
    public UIButton btnfast; //TIMESCALE 조절 버튼 3X
    public UIButton btnInGameSetting; //SETTING 버튼
    private bool isStarted; //게임 시작시 TRUE
    public UILabel lbStartCount; //시작시 타이머 3,2,1
    private float timeCount; //시간 카운트
    public UISprite iconStart; //START 아이콘    
    public int silver; //Hero를 Roll하기 위한 재화        
    public UILabel lbSilver;  //은화 표시
    public Action OnDisplaySilver; //은화 변경시 호출
    #endregion

    #region GAME_PROPERTY_FIELDS
    public Material whiteMaterial; 
    public int stageId; //스테이지ID
    public TestFence fence; //방벽
    public HeroPositionData[] heroPositionDatas; //영웅위치데이터
    public TestCommander commander; //지휘관
    public List<TestHero> listHeroes; //영웅들
    public List<GameObject> listFootholds; //영웅밑의 발판
    public List<TestMonster> listMonsters; //몬스터들
    public Action<int> OnStopSpawnMonster; //몬스터 소환 중지시 호출
    public Action OnMonsterRemovedFromList; //몬스터 제거시 호출
    #endregion

    void Start()
    {
        SoundBgManager.instance.StopAnimAudio();
        SoundInGameBgManager.instance.PlayAnimAudio();

        if (DataManager.GetInstance().userInfo.isBgmOn == true)
        {
            SoundInGameBgManager.instance.MuteAudio(false);
        }
        else
        {
            SoundInGameBgManager.instance.MuteAudio(true);
        }

        if (DataManager.GetInstance().userInfo.isFXsoundOn == true)
        {
            SoundEffectManager.instance.MuteAudio(false);
        }
        else
        {
            SoundEffectManager.instance.MuteAudio(true);
        }

        easyObjectPool.Init(); silver = 30;

        OnDisplaySilver = () =>
        {
            lbSilver.text = silver.ToString();
        };

        OnDisplaySilver();

        SetTimeScale(1f);

        if (uI_FenceHp != null)
        {
            uI_FenceHp.Init();
        }
        else
        {
            var prefab = (GameObject)(from obj in App.instance.resource
                                      where obj.name == "UI_FenceHp"
                                      select obj).FirstOrDefault();
            this.uI_FenceHp = Instantiate(prefab).GetComponent<UI_FenceHp>();

            this.uI_FenceHp.transform.SetParent(this.uiRoot.transform);
            this.uI_FenceHp.transform.localScale = Vector3.one;
            this.uI_FenceHp.transform.localPosition = Vector3.zero;

            uI_FenceHp.Init();
        }



        listHeroes = new List<TestHero>();
        listFootholds = new List<GameObject>();

        GetStageNumber(App.instance.stageNumber); //App에서 몇스테이지 인지 받아옴

        DisplayTotalMonsterCount(); //총 몬스터 수 표시

        OnMonsterRemovedFromList = () =>
        {
            this.monsterCount -= 1;
            silver += 5;
            OnDisplaySilver();
            DataManager.GetInstance().AddMonsterKiilCount(); //userInfo에 kiilCount 추가
            DisplayCurrentMonsterCount(); //현재 몬스터 숫자 표시
            if (App.instance.gpgsLogin.firbaseUser != null) //firebase의 이벤트에 killCount추가
            {
                Firebase.Analytics.Parameter[] param =
                {
                 new Firebase.Analytics.Parameter("killcount", DataManager.GetInstance().userInfo.monsterKillCount)
                };

                Firebase.Analytics.FirebaseAnalytics.LogEvent("killmonster", param);
            }
        };

        this.btnStart.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            timeCount = 0;
        }));
        btnNormalSpeed.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            SetTimeScale(1f);

        }));
        btnfast.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            //3배속으로 빠르게 플레이
            SetTimeScale(3f);
        }));

        AddEventsInGameButtons(); //게임 화면내 버튼에 이벤트 추가

        uIPopup_Victory.OnBtnLobbyClicked = () =>
        {
            SceneManager.LoadScene("Lobby2");
        };
        uIPopup_Fail.OnBtnLobbyClicked = () =>
        {
            SceneManager.LoadScene("Lobby2");
        };

        if (isStarted != true)
        {
            StartGame(stageId);
            SummonMonsters();
            isStarted = true;
        }

    }

    private void AddEventsInGameButtons()
    {
        btnInGameSetting.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();


            //this.uiPopup_InGameSetting.gameObject.SetActive(true);
            //this.uiPopup_InGameSetting.Init();
            if (this.uiPopup_InGameSetting != null)
            {
                this.uiPopup_InGameSetting.Open();
            }
            else
            {
                //var prefab = (GameObject)(from obj in App.instance.resource
                //                          where obj.name == "UIPopup_InGameSetting"
                //                          select obj).FirstOrDefault();
                var prefab = Resources.Load<GameObject>("Prefab/UI/UIPopup_InGameSetting");
                this.uiPopup_InGameSetting = Instantiate<GameObject>(prefab).GetComponent<UIPopup_InGameSetting>();

                this.uiPopup_InGameSetting.transform.SetParent(this.uiRoot.transform);
                this.uiPopup_InGameSetting.transform.localScale = Vector3.one;
                this.uiPopup_InGameSetting.transform.localPosition = Vector3.zero;

                this.uiPopup_InGameSetting.CheckMusic();

                this.uiPopup_InGameSetting.Init();
            }
        }));
    }
    private void StartGame(int stage)
    {
        LoadHudText();
        LoadStage(stage);
        LoadFence(stage);
        LoadCommander(stage);
        LoadHeroPositionAndType(stage);
        LoadFootholds(stage);
        uI_UserInteraction.Init();
        SetUserInteractionCallbacks();
        uI_UserInteraction.Check();
    }

    public void SummonMonsters()
    {
        StartCoroutine(SummonMonstersImpl());
    }
    IEnumerator SummonMonstersImpl()
    {
        this.lbStartCount.gameObject.SetActive(true);
        timeCount = 4;
        while (true)
        {
            yield return null;
            timeCount -= Time.deltaTime;
            this.lbStartCount.text = Math.Truncate(timeCount).ToString();
            if (timeCount < 1)
            {
                lbStartCount.text = "START";
                //    iconStart.material.DOColor(Color.black, 1);
                //    this.lbStartCount.gameObject.SetActive(false);
            }
            if (timeCount < 0)
            {
                LoadAllMonster(stageId);

                commander.SetListMonsters(listMonsters);
                foreach (var hero in listHeroes)
                {
                    hero.SetListMonsters(listMonsters);
                }
                StartCoroutine(CheckGameEnd());
                this.lbStartCount.gameObject.SetActive(false);
                break;
            }
        }
    }

    private void SetUserInteractionCallbacks()
    {
        uI_UserInteraction.OnPress = () =>
        {
            uI_UserInteraction.ChangeState(UI_UserInteraction.eTouchState.PRESS);
        };
        uI_UserInteraction.OnDragHero = () =>
        {
            uI_UserInteraction.ChangeState(UI_UserInteraction.eTouchState.DRAG);
        };
        uI_UserInteraction.OnPressNothing = () =>
        {
            uI_UserInteraction.ChangeState(UI_UserInteraction.eTouchState.PRESS_NOTHING);
        };
        uI_UserInteraction.OnRelease = () =>
        {
            uI_UserInteraction.ChangeState(UI_UserInteraction.eTouchState.RELEASE);
        };      
        uI_UserInteraction.OnChangeHero = (hero) =>
        {
            if (silver >= 10)
            {
                silver -= 10;
                OnDisplaySilver();

                if (hero.data.type < 6)
                {
                    var pos = hero.defaultPos;
                    listHeroes.Remove(hero);
                    hero.HideLevel();
                    EasyObjectPool.instance.ReturnObjectToPool(hero.gameObject);

                    var rand = UnityEngine.Random.Range(1, 6);

                    //rand = 5;
                    LoadHero(rand, pos);
                    CheckHeroCount();
                    uI_UserInteraction.HideHeroRandButton();
                }
            }
            //Debug.Log($"ChangeHero!------------hero:{hero}-------newHero:{newHero}-----heroPos: {pos}");
        };

        uI_UserInteraction.OnMergeHeroWithUiThumbnail = (hero) =>
        {
            Debug.Log($"썸네일과 히어로를 합치고 있습니다-------hero:{hero}");
            listHeroes.Remove(hero);
            var pos = hero.transform.position;
            hero.HideLevel();
            EasyObjectPool.instance.ReturnObjectToPool(hero.gameObject);
            var newType = hero.data.type + 5;
            LoadHero(newType, pos);
        };

        uI_UserInteraction.OnMergeHeroWithGameObject = (thisHero, otherHero) =>
        {
            Debug.Log($"현재 나와있는 캐릭터끼리 합치고 있습니다-------thisHero{thisHero}, otherHero{otherHero}");
            var pos = otherHero.transform.position;
            listHeroes.Remove(thisHero);
            listHeroes.Remove(otherHero);
            LoadHero(thisHero.data.type + 5, pos);
            thisHero.HideLevel();
            EasyObjectPool.instance.ReturnObjectToPool(thisHero.gameObject);
            otherHero.HideLevel();
            EasyObjectPool.instance.ReturnObjectToPool(otherHero.gameObject);
            uI_UserInteraction.OnPressNothing();
        };
        uI_UserInteraction.OnCreateNewHero = () =>
        {
            if (silver >= 10)
            {
                silver -= 10;
                OnDisplaySilver();
                uI_UserInteraction.uI_BtnThumb.CreateNewHero();
            }
        };
        uI_UserInteraction.uI_BtnThumb.OnPutDownHero = (type, pos) =>
        {
            if (CheckHeroCount() < 4)
            {
                LoadHero(type, pos);
            }
            uI_UserInteraction.uI_BtnThumb.OnIdle();
        };
        uI_UserInteraction.useSkillAction = () =>
        {

            var prefab = (GameObject)(from obj in App.instance.resource
                                      where obj.name == "Jet"
                                      select obj).FirstOrDefault();

            EasyObjectPool.instance.MakePoolInfo("Jet", prefab);

            GameObject go = EasyObjectPool.instance.GetObjectFromPool("Jet", new Vector3(-0.15f, 7f, -32f), Quaternion.identity);
            go.GetComponent<JetMove>().JetSkill();
            go.GetComponent<JetMove>().jetcollAction = () =>
            {
                foreach (var monster in listMonsters)
                {
                    monster.Hit(commander.calculatedDamage * 5);
                }
            };
        };
    }

    
    //HudText
    private void DisplayHudText(UIRoot uiRoot, Vector3 monsterPos, float damage)
    {
        Vector3 v = mainCam.WorldToScreenPoint(monsterPos);
        //Debug.Log(v);

        v.x = (v.x / Screen.width) * 1080;
        v.y = (v.y / Screen.height) * 1920;
        v.z = 0;
        var pos = v - new Vector3(1080 / 2, 1920 / 2, 0);
        //Debug.Log(v);
        //Debug.Log(v - new Vector3(1080 / 2, 1920 / 2, 0));
        var hudTextGo = EasyObjectPool.instance.GetObjectFromPool("UI_HudText1", Vector3.zero, Quaternion.identity);
        var hud = hudTextGo.GetComponent<UI_HudText>();
        hud.Init(uiRoot, pos, damage);
        hud.OnTweenEndCall = () =>
        {
            EasyObjectPool.instance.ReturnObjectToPool(hud.gameObject);
        };
        hud.Play();

    }


    //현재 Hero수 체크
    private int CheckHeroCount()
    {
        for (int i = 0; i < listHeroes.Count; i++)
        {
            Debug.Log($"Name:{listHeroes[i].name},Position:{listHeroes[i].transform.position},DefaultPos:{listHeroes[i].defaultPos}");
        }
        return listHeroes.Count;
    }
    public void GetStageNumber(int stageNumber)
    {
        this.stageId = stageNumber;
    }

    //배경, 방벽, NavMesh 로딩
    #region LOAD_PROPS
    public void LoadHudText()
    {
        var prefab = (GameObject)(from obj in App.instance.resource
                                  where obj.name == "UI_HudText1"
                                  select obj).FirstOrDefault();
        EasyObjectPool.instance.MakePoolInfo("UI_HudText1", prefab, 10);
    }
    public void LoadNavMesh()
    {
        UnityEngine.Object prefab = (from obj in App.instance.resource
                                     where obj.name == "NavMeshSurface"
                                     select obj).FirstOrDefault();
        var go = (GameObject)Instantiate(prefab);
    }
    public TestStage LoadStage(int number)
    {
        GameObject go = new GameObject(DataManager.GetInstance().dicStageData[number].name);
        UnityEngine.Object prefab = (from obj in App.instance.resource
                                     where obj.name == DataManager.GetInstance().dicStageData[number].prefab_name
                                     select obj).FirstOrDefault();
        GameObject model = (GameObject)Instantiate(prefab);
        model.transform.SetParent(go.transform, false);

        go.transform.position = new Vector3(DataManager.GetInstance().dicStageData[number].x, DataManager.GetInstance().dicStageData[number].y, DataManager.GetInstance().dicStageData[number].z);
        TestStage stage = go.AddComponent<TestStage>();
        return stage;
    }
    private void LoadFence(int number)
    {
        var fence_id = DataManager.GetInstance().dicFencePositionData[number].fence_id;
        var fence_position_data = DataManager.GetInstance().dicFencePositionData[number];
        var data = DataManager.GetInstance().dicFenceData[fence_id];
        UnityEngine.Object prefab = (from obj in App.instance.resource
                                     where obj.name == data.prefab_name
                                     select obj).FirstOrDefault();
        GameObject go = (GameObject)Instantiate(prefab);
        go.transform.SetParent(go.transform, false);
        go.transform.position = new Vector3(fence_position_data.x, fence_position_data.y, fence_position_data.z);
        TestFence fence = go.AddComponent<TestFence>();
        fence.Init(data);
        this.fence = fence;

        this.fence.OnChangeFenceHp = (percentage) =>
        {
            Debug.Log($"OnChangeFenceHp:{percentage}");
            uI_FenceHp.ChangeFillAmount(percentage);
        };

        fence.OnMonsterRange = (gameObject) =>
        {
            if (gameObject != null && gameObject.tag == "Monster")
            {
                gameObject.GetComponent<TestMonster>().ChangeBehavior(TestMonster.eMonsterState.ATTACK);
            }
        };
    }
    #endregion

    //지휘관 로딩
    #region LOAD_COMMANDER
    private void LoadCommander(int number)
    {
        var selectedData = (from info in DataManager.GetInstance().dicCommanderinfo
                            where info.Value.isSelected == true
                            select info.Value).FirstOrDefault();
        var commander_id = selectedData.commander_id;
        var data = DataManager.GetInstance().dicCommanderData[commander_id];
        UnityEngine.Object prefab = (from obj in App.instance.resource
                                     where obj.name == data.prefab_name
                                     select obj).FirstOrDefault();
        GameObject go = (GameObject)Instantiate(prefab);

        go.transform.position = new Vector3(DataManager.GetInstance().dicCommanderPositionData[number].x, DataManager.GetInstance().dicCommanderPositionData[number].y, DataManager.GetInstance().dicCommanderPositionData[number].z);

        this.commander = go.AddComponent<TestCommander>();

        commander.OnTargetFound = () =>
        {
            commander.ChangeBehavior(TestCommander.eCommanderState.ATTACK);
        };
        commander.OnAttackComplete = () =>
        {
            //Debug.Log("OnAttackComplete");

            commander.ChangeBehavior(TestCommander.eCommanderState.IDLE);
        };
        commander.OnIdleComplete = () =>
        {
            commander.ChangeBehavior(TestCommander.eCommanderState.SEARCH);
        };

        commander.Init(data, listMonsters); //Init

        if (commander.effectEvents != null)
        {
            commander.effectEvents.rangeMonstersAction = (listMonsters) =>
            {
                for (int i = 0; i < listMonsters.Count; i++)
                {
                    int capt = i;
                    listMonsters[capt].GetComponent<TestMonster>().OnDamagedMonster(commander.calculatedDamage);
                }
            };
            commander.effectEvents.dotDmgAction = (TargetMonster) =>
            {
                for (int i = 0; i < TargetMonster.Count; i++)
                {
                    int capt = i;
                    TargetMonster[capt].GetComponent<TestMonster>().OnDotDamageMonster(commander.calculatedDamage, 5, 0.2f); //도트(데미지,카운트,시간)
                }
            };

            commander.effectEvents.aspdUpAction = (percentage) =>
            {
                //commander.SetAspdUpAura();               
            };

        }
    }
    #endregion

    //영웅 로딩
    #region LOAD_HERO
    public void LoadFootholds(int stage)
    {
        heroPositionDatas = (from data in DataManager.GetInstance().dicHeroPositionData
                             where data.Value.stage_id == stage
                             select data.Value).ToArray();

        var prefab = (GameObject)(from obj in App.instance.resource
                                  where obj.name == "Foothold01"
                                  select obj).FirstOrDefault();
        for (int i = 0; i < 4; i++)
        {
            var foothold = Instantiate(prefab);
            var pos = new Vector3(heroPositionDatas[i].x, heroPositionDatas[i].y, heroPositionDatas[i].z);
            foothold.transform.position = pos;
            listFootholds.Add(foothold);
        }
    }
    public void LoadHeroPositionAndType(int stage)
    {
        //영웅 위치정보 찾기
        heroPositionDatas = (from data in DataManager.GetInstance().dicHeroPositionData
                             where data.Value.stage_id == stage
                             select data.Value).ToArray();

        //영웅 4명 랜덤으로 뽑기 위한 번호배열생성
        int[] arrHeroNumber = new int[4];
        for (int i = 0; i < 4; i++)
        {
            arrHeroNumber[i] = UnityEngine.Random.Range(1, 6);
            var pos = new Vector3(heroPositionDatas[i].x, heroPositionDatas[i].y, heroPositionDatas[i].z);
            LoadHero(arrHeroNumber[i], pos);
        }
    }
    public void LoadHero(int number, Vector3 pos)
    {
        var data = DataManager.GetInstance().dicHeroData[number];
        //ObjectPoolMaker.GetInstance().MakePool(data.name, data.prefab_name);
        var prefab = (GameObject)(from obj in App.instance.resource
                                  where obj.name == data.prefab_name
                                  select obj).FirstOrDefault();

        Debug.Log($"{EasyObjectPool.instance}");
        Debug.Log($"{data.name}");
        Debug.Log($"{prefab.name}");

        EasyObjectPool.instance.MakePoolInfo(data.name, prefab);

        GameObject go = EasyObjectPool.instance.GetObjectFromPool(data.name, pos, Quaternion.identity);

        var hero = go.GetComponent<TestHero>();
        if (hero == null)
        {
            hero = go.AddComponent<TestHero>();
        }
        listHeroes.Add(hero);

        hero.OnTargetFound = () =>
        {
            hero.ChangeBehavior(TestHero.eHeroState.ATTACK);
        };
        hero.OnAttackComplete = () =>
        {
            //Debug.Log("OnAttackComplete");

            hero.ChangeBehavior(TestHero.eHeroState.IDLE);
        };
        hero.OnIdleComplete = () =>
        {
            hero.ChangeBehavior(TestHero.eHeroState.SEARCH);
        };

        hero.Init(data, listMonsters, uiRoot); //Init

        if (hero.effectEvents != null)
        {
            hero.effectEvents.rangeMonstersAction = (listMonsters) =>
            {
                for (int i = 0; i < listMonsters.Count; i++)
                {
                    int capt = i;
                    listMonsters[capt].GetComponent<TestMonster>().OnDamagedMonster(hero.calculatedDamage);
                }
            };
            hero.effectEvents.dotDmgAction = (TargetMonster) =>
            {
                for (int i = 0; i < TargetMonster.Count; i++)
                {
                    int capt = i;
                    TargetMonster[capt].GetComponent<TestMonster>().OnDotDamageMonster(hero.calculatedDamage, 5, 1);  //도트(데미지,카운트,시간)
                    Debug.Log("도드 레벨 111111111 액션 실행");
                }
            };
            hero.effectEvents.dotDmgActionLv2 = (TargetMonster) =>
            {
                for (int i = 0; i < TargetMonster.Count; i++)
                {
                    int capt = i;
                    var Lv2dmg = commander.calculatedDamage * 2;
                    TargetMonster[capt].GetComponent<TestMonster>().OnDotDamageMonster(Lv2dmg, 5, 1);  //도트(데미지,카운트,시간)
                    Debug.Log("도드 레벨 22222222 액션 실행" + Lv2dmg);
                }
            };
            hero.effectEvents.dotDmgActionLv3 = (TargetMonster) =>
            {
                for (int i = 0; i < TargetMonster.Count; i++)
                {
                    int capt = i;
                    var Lv2dmg = commander.calculatedDamage * 2;
                    var random = UnityEngine.Random.Range(1, 4);
                    if (random == 1)
                    {
                        TargetMonster[capt].GetComponent<TestMonster>().hp = 0;
                        Debug.Log("~~~~~~~~~~~즉사발생~~~~~~~~~~~~");
                    }
                    var Lv3dmg = commander.calculatedDamage * 2;
                    TargetMonster[capt].GetComponent<TestMonster>().OnDotDamageMonster(Lv3dmg, 5, 1);
                    Debug.Log("도드 레벨 333333 액션 실행");
                }
            };
            hero.effectEvents.HpPercentDmgAction = (TargetMonster) =>
            {
                foreach (var monster in TargetMonster)
                {
                    monster.GetComponent<TestMonster>().HitProportional(20);
                }
            };
            //hero06_2
            hero.effectEvents.attackSpeedUpAction = (TargetMonster) =>
            {
                hero.BuffSelfApsd(50);

                for (int i = 0; i < TargetMonster.Count; i++)
                {
                    int capt = i;
                    TargetMonster[capt].GetComponent<TestMonster>().OnDamagedMonster(hero.calculatedDamage);
                }
            };
            hero.effectEvents.aspdUpAction = (percentage) =>
            {
                //hero.SetAspdUpAura();
                for (int i = 0; i < listHeroes.Count; i++)
                {
                    int capt = i;
                    this.listHeroes[capt].SetAspdUpAura();
                    this.listHeroes[capt].BuffAspd(percentage);
                }
            };
            //Hero03_1
            hero.effectEvents.knockBackAttackAction = (monsterGo) =>
            {
                monsterGo.GetComponent<TestMonster>().ChangeBehavior(TestMonster.eMonsterState.KNOCKBACK);
                monsterGo.GetComponent<TestMonster>().Hit(hero.calculatedDamage);
                Debug.Log($"Hero03 Damage{hero.calculatedDamage}");
            };
            //Hero03_2
            hero.effectEvents.slowAttackAction = (monsterGo) =>
            {
                monsterGo.GetComponent<TestMonster>().DebuffMoveSpeed(20, 10);
                hero.effectEvents.knockBackAttackAction(monsterGo);
                //monsterGo.GetComponent<TestMonster>().Hit(hero.data.damage);
            };
            //Hero03_3
            hero.effectEvents.stunAttackAction = (monsterGo) =>
            {
                monsterGo.GetComponent<TestMonster>().ChangeBehavior(TestMonster.eMonsterState.STUN);
                hero.effectEvents.slowAttackAction(monsterGo);
                //monsterGo.GetComponent<TestMonster>().Hit(hero.data.damage);
            };
        }
    }
    #endregion

    //몬스터 로딩
    #region LOAD_MONSTER
    public void LoadAllMonster(int number)
    {
        listMonsters = new List<TestMonster>();
        var waveDatas = (from data in DataManager.GetInstance().dicWaveData
                         where data.Value.group_id == number
                         select data.Value).ToArray();
        SpawnMonsters(waveDatas);
    }
    public void SpawnMonsters(WaveData[] data)
    {
        List<Coroutine> listCoroutines = new List<Coroutine>();
        for (int i = 0; i < data.Length; i++)
        {
            listCoroutines.Add(StartCoroutine(SpawnMonsterImpl(i, data[i])));
        }

        OnStopSpawnMonster = (index) =>
        {
            StopCoroutine(listCoroutines[index]);
        };

    }
    IEnumerator SpawnMonsterImpl(int index, WaveData waveData)
    {
        var data = DataManager.GetInstance().dicMonsterData[waveData.monster_id];
        GameObject prefab = (GameObject)(from obj in App.instance.resource
                                         where obj.name == data.prefab_name
                                         select obj).FirstOrDefault();
        EasyObjectPool.instance.MakePoolInfo(data.name, prefab);

        var monsterPosition = new Vector3(waveData.x, waveData.y, waveData.z);
        for (int i = 0; i < waveData.count; i++)
        {

            GameObject go = EasyObjectPool.instance.GetObjectFromPool(data.name, monsterPosition, Quaternion.identity);

            TestMonster monster = go.GetComponent<TestMonster>();
            if (monster == null)
            {
                monster = go.AddComponent<TestMonster>();
            }

            var destination_x = UnityEngine.Random.Range(waveData.destination_x - 1, waveData.destination_x + 1);
            var destination = new Vector3(destination_x, waveData.destination_y, waveData.destination_z);

            monster.Init(destination, DataManager.GetInstance().dicMonsterData[waveData.monster_id], fence, whiteMaterial);

            monster.monsterAttackEvent.monsterAttackAction = () =>
            {
                fence.Hit(monster.data.damage);
            };

            monster.ChangeBehavior(TestMonster.eMonsterState.MOVE);

            listMonsters.Add(monster);

            monster.OnCompleteMove = () =>
            {
                monster.previousState = monster.currentState;
                monster.ChangeBehavior(TestMonster.eMonsterState.FOLLOW);
            };
            monster.OnCompleteKnockBack = () =>
            {
                if (monster.previousState == TestMonster.eMonsterState.MOVE)
                {
                    monster.previousState = TestMonster.eMonsterState.FOLLOW;
                }
                monster.ChangeBehavior(monster.previousState);
                monster.previousState = monster.currentState;
            };
            monster.OnCompleteStun = () =>
             {
                 monster.ChangeBehavior(monster.previousState);
                 monster.previousState = monster.currentState;
             };
            monster.OnMonsterDie = () =>
            {
                //Debug.Log($"OnMonsterDie{monster.name}");
                monster.previousState = monster.currentState;
                monster.ChangeBehavior(TestMonster.eMonsterState.DIE);
                bool isSuccess = listMonsters.Remove(monster);
                //Debug.Log($"isSuccess: {isSuccess}");
                OnMonsterRemovedFromList();
            };
            monster.OnMonsterDead = () =>
            {
                monster.previousState = monster.currentState;
                monster.ChangeBehavior(TestMonster.eMonsterState.IDLE);
                EasyObjectPool.instance.ReturnObjectToPool(monster.gameObject);
            };
            monster.OnDisplayHudText = (damage) =>
            {
                DisplayHudText(uiRoot, monster.transform.position, damage);
            };
            yield return new WaitForSeconds(waveData.term);
        }

        OnStopSpawnMonster(index);
    }
    #endregion

    
    #region DISPLAY_MONSTERCOUNT
    public void DisplayTotalMonsterCount()
    {
        var waveDatas = (from data in DataManager.GetInstance().dicWaveData
                         where data.Value.group_id == stageId
                         select data.Value).ToArray();

        foreach (var wave in waveDatas)
        {
            monsterCount += wave.count;
        }
        this.lbMonsterCount.text = monsterCount.ToString();
    }

    public void DisplayCurrentMonsterCount()
    {
        this.lbMonsterCount.text = monsterCount.ToString();
    }
    #endregion

    #region CHECK_GAME_WIN_LOSE
    IEnumerator CheckGameEnd()
    {
        while (true)
        {
            if (monsterCount == 0)
            {
                yield return new WaitForSeconds(1f);

                uIPopup_Victory.gameObject.SetActive(true);
                var rewardCash = DataManager.GetInstance().dicStageData[stageId].rewardCash;
                DataManager.GetInstance().SetStageCleared(stageId);
                DataManager.GetInstance().SaveStageClearInfo();
                DataManager.GetInstance().SetStageAchivementCleared(stageId);
                DataManager.GetInstance().SetMonsterKillCountAchivementCleared();
                DataManager.GetInstance().SaveUserInfo();
                uIPopup_Victory.Init(rewardCash);
                SetTimeScale(0);
                break;
            }
            else if (fence.hp <= 0)
            {
                yield return new WaitForSeconds(1f);

                uIPopup_Fail.gameObject.SetActive(true);
                var comfortCash = DataManager.GetInstance().dicStageData[stageId].comfortCash;
                DataManager.GetInstance().SaveUserInfo();
                uIPopup_Fail.Init(comfortCash);
                SetTimeScale(0);
                //Debug.Log("펜스 체력 0이하");
                break;
            }
            yield return null;
        }
    }
    #endregion

    //게임속도조절
    private void SetTimeScale(float num)
    {
        Time.timeScale = num;
    }
}
