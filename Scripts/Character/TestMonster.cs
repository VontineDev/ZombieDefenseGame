using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class TestMonster : MonoBehaviour
{
    public DefenseMonsterData data; //monster_data.json의 row
    public float hp; //몬스터의 현재hp Init함수에서 data.maxHp로 초기화됨    
    public float decreasedPercentage;
    public TestFence fence; //공격대상인 Fence

    public bool isTargetedByBomber; //폭탄병의 중복타겟이 되지 않기위한 변수

    public NavMeshAgent agent;  //NavAgent
    public Vector3 navDestination;  //NavAgent의 목적지
    public Animator animator;   //Animator
    public GameObject model;    //Monster의 model
    public BoxCollider boxCollider; //Collider, Fence와 충돌감지
    public SkinnedMeshRenderer skinnedMeshRenderer; //피격시 깜빡이는 효과를 위한 MeshRenderer    
    public Material defaultMaterial;    //기본 머티리얼
    public Material whiteMaterial;  //하얀 머티리얼 

    public MonsterAttackEvent monsterAttackEvent;
    public System.Action<float> OnAttackComplete;
    public System.Action OnCompleteMove;//NavMesh이동이 끝나면 호출   
    public System.Action<float> OnDamagedMonster;//맞은 시점에 호출    
    public System.Action<float, int, float> OnDotDamageMonster;
    public System.Action OnHit;//이벤트에서 공격시점에 호출
    public System.Action OnCompleteStun; //스턴이 끝나면 호출
    public System.Action OnCompleteKnockBack; //넉백이 끝나면 호출
    public System.Action OnMonsterDie; //몬스터 죽을 때 호출
    public System.Action OnDetargeted;
    public System.Action OnMonsterDead; //몬스터 죽고나면(애니메이션끝나고) 호출
    public System.Action<float> OnDisplayHudText; //HUD Text 띄울 때 호출
    public System.Action<float> AttackCplAction;

    private Coroutine coCheckHp; //자신의 체력을 확인하기위한 코루틴
    public Coroutine coHit; //피격시 몬스터 자체 이펙트 수행을 위한 코루틴
    
    private Coroutine coDebuffMoveSpeed; //이속감소 디버프 코루틴


    public enum eMonsterState { IDLE, MOVE, FOLLOW, ATTACK, DIE, KNOCKBACK, STUN } //몬스터 상태
    public eMonsterState previousState;
    public eMonsterState currentState;
    public enum eMonsterType { RunTwoAttack = 1, WalkOneAttack, CrawlBiting }   //몬스터 타입

    private Coroutine coBehavior;   //행동 코루틴
    void Start()
    {
        this.OnDamagedMonster = (damage) =>
        {
            //Debug.Log($"{this.name}몬스터가 데미지를받았다");

            Hit(damage);
        };
        this.OnDotDamageMonster = (damage, count, second) =>
        {
            HitDot(damage, count, second);
        };
    }

    #region INIT
    public void Init(Vector3 navDestination, DefenseMonsterData data, TestFence fence, Material mat)
    {
        this.navDestination = navDestination;

        this.data = data;
        this.hp = data.maxHp;
        this.fence = fence;
        this.whiteMaterial = mat;

        if (monsterAttackEvent == null)
        {
            this.monsterAttackEvent = this.gameObject.transform.GetChild(0).gameObject.AddComponent<MonsterAttackEvent>();
        }

        this.isTargetedByBomber = false;

        if (model == null)
        {
            this.model = this.transform.GetChild(0).gameObject;
        }
        this.model.transform.localPosition = Vector3.zero;

        if (agent == null)
        {
            this.agent = this.gameObject.AddComponent<NavMeshAgent>();
        }
        if (animator == null)
        {
            this.animator = this.gameObject.GetComponentInChildren<Animator>();
        }
        if (boxCollider == null)
        {
            this.boxCollider = this.gameObject.AddComponent<BoxCollider>();
            this.boxCollider.size = new Vector3(1, 3, 1);
            this.boxCollider.center = new Vector3(0, 1.2f, 0);
            this.boxCollider.isTrigger = true;
        }
        this.gameObject.layer = LayerMask.NameToLayer("Monster"); //레이어를 몬스터로

        this.skinnedMeshRenderer = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        this.defaultMaterial = this.skinnedMeshRenderer.material;

        this.CheckHp();
    }
    #endregion

    #region CHANGEBEHAVIOR
    public void ChangeBehavior(eMonsterState state)
    {
        SetNavAgentEnable(false);

        switch (state)
        {
            case eMonsterState.IDLE:
                {
                    Idle();
                }
                break;
            case eMonsterState.MOVE:
                {
                    Move();
                }
                break;
            case eMonsterState.FOLLOW:
                {
                    Follow();
                }
                break;
            case eMonsterState.ATTACK:
                {
                    Attack();
                }
                break;
            case eMonsterState.DIE:
                {
                    Die();
                }
                break;
            case eMonsterState.KNOCKBACK:
                {
                    KnockBack();
                }
                break;
            case eMonsterState.STUN:
                {
                    Stun();
                }
                break;
            default:
                break;
        }
    }
    #endregion

    #region IDLE
    private void Idle()
    {
        currentState = eMonsterState.IDLE;
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(IdleImpl());

    }
    IEnumerator IdleImpl()
    {
        this.animator.Play("Monster_Idle");
        yield return null;
    }
    #endregion

    #region MOVE
    // 몬스터가 이동하는 매서드 (NavMesh로)
    private void Move()
    {
        currentState = eMonsterState.MOVE;
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(MoveImpl());
    }
    IEnumerator MoveImpl() // 목표지점으로 이동하면서 목표지점에 도달하면 앞으로이동
    {
        SetNavAgentEnable(true);
        SetNavAgentDatas();
        switch ((eMonsterType)data.attackType)
        {
            case (eMonsterType.RunTwoAttack):
                {
                    this.animator.Play("Monster_Run");
                }
                break;
            case (eMonsterType.WalkOneAttack):
                {
                    this.animator.Play("Monster_Walk");
                }
                break;
            case (eMonsterType.CrawlBiting):
                {
                    this.animator.Play("Monster_Crawl");
                }
                break;
        }
        while (true)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    OnCompleteMove();
                    break;
                }
            }
            yield return null;
        }
    }
    #endregion

    #region FOLLOW

    private void Follow()  //펜스로 전진(-z방향)
    {
        currentState = eMonsterState.FOLLOW;
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(FollowImpl());
    }
    IEnumerator FollowImpl()
    {
        SetNavAgentEnable(false);

        switch ((eMonsterType)data.attackType)
        {
            case (eMonsterType.RunTwoAttack):
                {
                    this.animator.Play("Monster_Run");
                }
                break;
            case (eMonsterType.WalkOneAttack):
                {
                    this.animator.Play("Monster_Walk");
                }
                break;
            case (eMonsterType.CrawlBiting):
                {
                    this.animator.Play("Monster_Crawl");
                }
                break;
        }
        while (true)
        {
            yield return null;

            var moveSpeed = FormulaCalculator.GetInstance().CalculateDecreasedMoveSpeed(data.moveSpeed, decreasedPercentage);

            this.gameObject.transform.position += Vector3.back * moveSpeed * Time.deltaTime;

            this.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    #endregion

    #region ATTACK
    private void Attack()
    {
        currentState = eMonsterState.ATTACK;
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(AttackImpl());
    }
    IEnumerator AttackImpl()
    {
        while (true)
        {
            switch ((eMonsterType)data.attackType)
            {
                case (eMonsterType.RunTwoAttack):
                    {
                        this.animator.Play("Monster_TwoAttack");
                    }
                    break;
                case (eMonsterType.WalkOneAttack):
                    {
                        this.animator.Play("Monster_OneAttack");
                    }
                    break;
                case (eMonsterType.CrawlBiting):
                    {
                        this.animator.Play("Monster_Biting");
                    }
                    break;
            }

            //몬스터의 공격속도에 따라 다른 타이밍을 적용해보자
            yield return new WaitForSeconds(3f);
        }
    }
    #endregion //공격   

    #region DIE
    private void Die()
    {
        currentState = eMonsterState.DIE;
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(DieImpl());
    }
    IEnumerator DieImpl()
    {
        while (true)
        {
            this.skinnedMeshRenderer.material = defaultMaterial;
            //OnDetargeted();
            //타입에 따라 다르게 몬스터를 죽도록 해본다
            switch ((eMonsterType)data.attackType)
            {
                case (eMonsterType.RunTwoAttack):
                    {
                        this.animator.Play("Monster_Die1");
                    }
                    break;
                case (eMonsterType.WalkOneAttack):
                    {
                        this.animator.Play("Monster_Die2");
                    }
                    break;
                case (eMonsterType.CrawlBiting):
                    {
                        this.animator.Play("Monster_Die3");
                    }
                    break;
            }
            //Debug.Log("Monster Dieimpl--------------1");

            yield return new WaitForSeconds(2f);
            OnMonsterDead();
            //Debug.Log("Monster DieImpl--------------22");
            break;
        }


    }
    #endregion

    #region KNOCKBACK
    public void KnockBack()
    {
        if (currentState != eMonsterState.DIE && currentState != eMonsterState.IDLE)
        {
            currentState = eMonsterState.KNOCKBACK;
            if (coBehavior != null)
            {
                StopCoroutine(coBehavior);
            }
            coBehavior = StartCoroutine(KnockBackImpl());
        }
    }
    IEnumerator KnockBackImpl()
    {
        for (int i = 0; i < 20; i++)
        {
            this.transform.position += Vector3.forward * 10 * Time.deltaTime;
            yield return null;
        }
        OnCompleteKnockBack();
    }
    #endregion

    #region STUN
    private void Stun()
    {
        if (currentState != eMonsterState.DIE && currentState != eMonsterState.IDLE)
        {
            currentState = eMonsterState.KNOCKBACK;
            if (coBehavior != null)
            {
                StopCoroutine(coBehavior);
            }
            coBehavior = StartCoroutine(StunImpl());
        }
    }
    IEnumerator StunImpl()
    {
        this.animator.Play("Monster_Stun", -1, 0);
        DebuffMoveSpeed(100, 3);
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            yield return null;

            if (time > 3)
            {
                OnCompleteStun();
                break;
            }
        }
    }
    #endregion

    private void SetNavAgentEnable(bool isEnabled)
    {
        this.agent.enabled = isEnabled;
    }
    private void SetNavAgentDatas()
    {
        this.agent.destination = navDestination;
        var moveSpeed = FormulaCalculator.GetInstance().CalculateDecreasedMoveSpeed(data.moveSpeed, decreasedPercentage);
        this.agent.speed = moveSpeed;
        this.agent.stoppingDistance = 2f;
    }

    //이속감소 디버프 
    public void DebuffMoveSpeed(int percentage, int time)
    {
        if (coDebuffMoveSpeed != null)
        {
            StopCoroutine(coDebuffMoveSpeed);
        }
        coDebuffMoveSpeed = StartCoroutine(DebuffMoveSpeedImpl(percentage, time));
    }
    IEnumerator DebuffMoveSpeedImpl(int percentage, int time)
    {
        float timeCount = 0;
        while (true)
        {
            yield return null;
            decreasedPercentage = percentage;
            timeCount += Time.deltaTime;
            if (timeCount >= time)
            {
                decreasedPercentage = 0;
                break;
            }
        }
    }

    #region CHECKHP
    private void CheckHp()
    {
        coCheckHp = StartCoroutine(CheckHpImpl());
    }
    IEnumerator CheckHpImpl()
    {
        while (true)
        {
            yield return null;
            if (this.hp <= 0)
            {
                this.OnMonsterDie();
                break;
            }
        }
    }
    #endregion

    #region HIT
    public void Hit(float damage)
    {
        if (this.hp > 0)
        {
            this.hp -= damage;
            OnDisplayHudText(damage);
            if (coHit != null)
            {
                StopCoroutine(coHit);
            }
            coHit = StartCoroutine(HitEffectImpl());
        }
    }
    IEnumerator HitEffectImpl()
    {
        //메시를 하얗게 바꿔봄
        skinnedMeshRenderer.material = whiteMaterial;

        yield return new WaitForSeconds(0.2f);
        skinnedMeshRenderer.material = defaultMaterial;
    }
    #endregion

    #region HIT_DOT
    public void HitDot(float damage, int count, float seconds)
    {
        StartCoroutine(HitDotImpl(damage, count, seconds));
    }
    IEnumerator HitDotImpl(float damage, int count, float seconds)
    {
        int i = 0;
        while (true)
        {
            Hit(damage);
            i++;
            if (i == count)
            {
                break;
            }
            yield return new WaitForSeconds(seconds);
        }
    }
    #endregion

    #region HIT_PROPORTIONAL
    public void HitProportional(float percentage)
    {
        var damage = data.maxHp * (100 - percentage) * 100;
        if (this.hp > 0)
        {
            this.hp -= damage;
            
            if (coHit != null)
            {
                StopCoroutine(coHit);
            }
            coHit = StartCoroutine(HitEffectImpl());
        }
    }
    #endregion
}