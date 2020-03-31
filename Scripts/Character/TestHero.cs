using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using MarchingBytes;
public class TestHero : MonoBehaviour
{
    public UIPopup_HeroLevel uIPopup_HeroLevel; //영웅 레벨(별) UI
    public UIRoot uiRoot;
    public HeroData data; //영웅 데이터

    public float calculatedDamage; //실제 데미지
    public float calculatedAspd; //실제 공속
    public GameObject aspdAura;

    public BoxCollider boxCollider;
    public Animator animator;
    public GameObject model;

    public List<TestMonster> listMonsters; //몬스터 리스트
    public TestMonster targetedMonster; //타겟

    public int bySelfDecreaedDelay; //자신에 의한 공속증가치
    public int byOtherDecreasedDelay; //타인에 의한 공속증가치


    public Action OnTargetFound; //타겟 찾으면 호출
    public Action OnAttackComplete; //공격이 끝나면 호출
    public Action OnIdleComplete; //아이들링(공격 후 딜레이) 끝나면 호출

    public System.Action OnDragNow; 
    public System.Action<TestHero, TestHero> OnMergeAccepted;
    
    public EffectEvents effectEvents;
    
    public bool isActivatedMenu;
    public Vector3 defaultPos;

    private Coroutine coBuffAspdByOther; 

    
    public eHeroState currentState; //현재상태
    public enum eHeroState { SEARCH, ATTACK, IDLE, USESKILL, DRAGGED, DROPPED }
    private Coroutine coBehavior; //상태 코루틴
    private void Start()
    {
        StartCoroutine(CheckState());
    }
    IEnumerator CheckState()
    {
        while (true)
        {
            //Debug.Log(this.currentState.ToString());
            yield return new WaitForSeconds(1f);
        }
    }

    #region INIT
    public void Init(HeroData data, List<TestMonster> listMonsters, UIRoot uiRoot)
    {
        //가져온 데이터를 참조합니다
        this.uiRoot = uiRoot;
        this.data = data;
        this.bySelfDecreaedDelay = 0;
        this.byOtherDecreasedDelay = 0;
        this.calculatedDamage = FormulaCalculator.GetInstance().AllCalculatedDamage_Hero(data.id);
        this.calculatedAspd = FormulaCalculator.GetInstance().AllCalculatedAspd_Hero(data.id);
        this.defaultPos = this.transform.position;
        if (this.model == null)
        {
            this.model = this.gameObject.transform.GetChild(0).gameObject;
        }
        this.model.transform.localPosition = Vector3.zero;

        if (this.animator == null)
        {
            this.animator = this.gameObject.GetComponentInChildren<Animator>();
        }
        if (this.GetComponentInChildren<EffectEvents>() != null)
        {
            this.effectEvents = this.GetComponentInChildren<EffectEvents>();
        }
        //레이어를 Hero로 만듦
        this.gameObject.layer = LayerMask.NameToLayer("Hero");

        if (this.boxCollider == null)
        {
            //boxCollider를 붙입니다(Hero Merge를 위함)
            this.boxCollider = this.gameObject.AddComponent<BoxCollider>();
            this.boxCollider.size = new Vector3(2f, 2, 2f);
            this.boxCollider.center = new Vector3(0, 1.2f, 0);
            this.boxCollider.isTrigger = true;
        }
        //몬스터 리스트 참조를 가져옵니다
        this.listMonsters = listMonsters;

        if (uIPopup_HeroLevel == null)
        {
            NewUiPopup_HeroLevel();
        }
        SetPositionUIPopup_Hero();
        var type = 0;
        if (data.type < 6)
        {
            type = 1;
        }
        else if (data.type < 11)
        {
            type = 2;
        }
        else if (data.type < 16)
        {
            type = 3;
        }
        uIPopup_HeroLevel.SetLevel(type);

        this.ChangeBehavior(eHeroState.SEARCH);
    }
    #endregion

    #region CHANGEBEHAVIOR
    public void ChangeBehavior(eHeroState state)
    {

        switch (state)
        {
            case eHeroState.SEARCH:
                {
                    Search();
                }
                break;
            case eHeroState.ATTACK:
                {
                    Attack();
                }
                break;
            case eHeroState.IDLE:
                {
                    Idle();
                }
                break;
            case eHeroState.USESKILL:
                {
                    UseSkill();
                }
                break;
            case eHeroState.DRAGGED:
                {
                    Dragged();
                }
                break;
            case eHeroState.DROPPED:
                {
                    Dropped();
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region SEARCH
    private void Search()
    {
        currentState = eHeroState.SEARCH;
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(SearchImpl());
    }
    IEnumerator SearchImpl()
    {
        if (listMonsters.Count == 0)
        {
            ChangeBehavior(eHeroState.IDLE);
        }

        var layerMask = 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Obstacle");
        while (true)
        {
            this.animator.Play("Search", -1, 0);
            //Debug.Log("Search While문");
            //Debug.Log($"{listMonsters.Count}");
            if (listMonsters.Count > 0)
            {
                var shortDis = Vector3.Distance(gameObject.transform.position, listMonsters[0].transform.position); // 첫번째를 기준으로 잡아주기 

                var foundMonster = listMonsters[0]; // 첫번째를 먼저 

                foreach (var monster in listMonsters)
                {
                    float dis = Vector3.Distance(gameObject.transform.position, monster.transform.position);

                    if (dis < shortDis) // 위에서 잡은 기준으로 거리 재기
                    {
                        foundMonster = monster;
                        shortDis = dis;
                    }
                }

                //Debug.LogFormat("찾은 몬스터 {0}", foundMonster.data.prefab_name);

                //OnTargetFound();

                var dir = foundMonster.transform.position - this.transform.position;
                var dis2 = dir.magnitude;
                RaycastHit hit;
                Debug.DrawRay(this.transform.position, dir * data.atkRange, Color.red, 1f);

                if (Physics.Raycast(this.transform.position, dir, out hit, data.atkRange, layerMask))
                {
                    //Debug.Log($"{hit.transform.gameObject.name} 부딪힌녀석");

                    //Debug.Log($"dis:{dis2}/atkRange:{data.atkRange}");
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Monster"))
                    {
                        //Debug.Log($"{hit.transform.gameObject.name} 부딪힌녀석2222222222222");
                        this.targetedMonster = hit.transform.gameObject.GetComponentInChildren<TestMonster>();
                        OnTargetFound();
                        break;
                    }
                }
                //Debug.Log(hit.transform.gameObject.name);
            }
            yield return null;
        }
    }
    #endregion

    #region ATTACK
    public void Attack()
    {
        currentState = eHeroState.ATTACK;
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(AttackImpl());
    }
    IEnumerator AttackImpl()
    {
        var time = 0.5f;
        var monster = targetedMonster;

        while (true)
        {
            this.gameObject.transform.root.LookAt(monster.transform);

            if (effectEvents != null)
            {
                effectEvents.monster = monster.gameObject;
            }
            this.animator.speed = 1.5f;
            this.animator.Play($"{data.anim_name}_Attack", -1, 0);

            var runtimeAC = this.animator.runtimeAnimatorController;
            for (int i = 0; i < runtimeAC.animationClips.Length; i++)
            {
                //Debug.Log($"{data.prefab_name}----------{runtimeAC.animationClips[i].name}");
                //Debug.Log(runtimeAC.animationClips[0].name);
                if (runtimeAC.animationClips[i].name == $"{data.anim_name}_Attack")
                {
                    time = runtimeAC.animationClips[i].length;
                }
            }

            yield return new WaitForSeconds(time);
            OnAttackComplete();
            break;
        }
    }
    #endregion

    #region IDLE
    public void Idle()
    {
        currentState = eHeroState.IDLE;
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(IdleImpl());
    }
    IEnumerator IdleImpl()
    {
        targetedMonster = null;

        //Debug.Log("IdleImpl");
        while (true)
        {
            this.animator.Play($"{data.anim_name}_Idle");
            //Debug.Log("아이들링");
            var idleTime = FormulaCalculator.GetInstance().CalculateIdleTime(this.calculatedAspd, this.bySelfDecreaedDelay, this.byOtherDecreasedDelay);
            Debug.Log($"idleTime: {idleTime}");

            yield return new WaitForSeconds(idleTime);
            OnIdleComplete();
            break;
        }

    }
    #endregion

    #region USESKILL
    public void UseSkill()
    {
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(UseSkillImpl());
    }
    IEnumerator UseSkillImpl()
    {
        yield return null;
    }
    #endregion

    #region DRAGGED
    public void Dragged()
    {
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(DragImpl());
    }
    IEnumerator DragImpl()
    {
        yield return null;
    }
    #endregion

    #region DROPPED

    public void Dropped()
    {
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(DroppedImpl());
    }
    IEnumerator DroppedImpl()
    {
        yield return null;
    }
    #endregion

    public void SetListMonsters(List<TestMonster> listMonsters)
    {
        this.listMonsters = listMonsters;
    }

    //영웅 레벨(별) 표시 UI 생성
    private void NewUiPopup_HeroLevel()
    {
        var prefab = (GameObject)(from obj in App.instance.resource
                                  where obj.name == "UIPopup_HeroLevel"
                                  select obj).FirstOrDefault();
        this.uIPopup_HeroLevel = Instantiate(prefab).GetComponent<UIPopup_HeroLevel>();
    }
    //영웅 레벨(별) 재설정
    private void SetPositionUIPopup_Hero()
    {
        uIPopup_HeroLevel.transform.parent = uiRoot.transform;
        uIPopup_HeroLevel.transform.localScale = Vector3.one;

        var pos = this.defaultPos;
        Vector3 v = Camera.main.WorldToScreenPoint(pos);
        //Debug.Log(v);
        v.x = (v.x / Screen.width) * 1080;
        v.y = (v.y / Screen.height) * 1920;
        //Debug.Log(v);
        //Debug.Log(v - new Vector3(1080 / 2, 1920 / 2, 0));
        uIPopup_HeroLevel.transform.localPosition = v - new Vector3(1080 / 2, 1920 / 2, 0) + 50 * Vector3.down;
    }

    public void HideLevel()
    {
        this.uIPopup_HeroLevel.SetLevel(0);
    }

    //공속 증가시 오라 표시
    public void SetAspdUpAura()
    {
        var prefab = (GameObject)(from obj in App.instance.resource
                                  where obj.name == "AspdAura"
                                  select obj).FirstOrDefault();
        EasyObjectPool.instance.MakePoolInfo("AspdAura", prefab);
        if (this.aspdAura == null)
        {
            this.aspdAura = EasyObjectPool.instance.GetObjectFromPool("AspdAura", this.defaultPos, Quaternion.identity);
        }
    }

    //자신에 의한 공속버프
    public void BuffSelfApsd(int percentage)
    {
        if (percentage < 100)
        {
            this.bySelfDecreaedDelay = percentage;
        }
    }
    //타인에 의한 공속버프
    public void BuffAspd(int percentage)
    {
        if (byOtherDecreasedDelay < percentage)
        {
            this.byOtherDecreasedDelay = percentage;

            if (coBuffAspdByOther != null)
            {
                StopCoroutine(coBuffAspdByOther);
            }
            coBuffAspdByOther = StartCoroutine(BuffAspdTimer());
        }
    }

    IEnumerator BuffAspdTimer()
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            if (time > 30)
            {
                this.byOtherDecreasedDelay = 0;
                break;
            }
            yield return null;
        }
    }
}
