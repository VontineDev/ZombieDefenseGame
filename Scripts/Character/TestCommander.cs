using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class TestCommander : MonoBehaviour
{
    public float idleTime;
    public CommanderData data;
    public float calculatedDamage;
    public float calculatedAspd;
    public BoxCollider boxCollider;
    public Animator animator;
    public List<TestMonster> listMonsters;
    public TestMonster targetedMonster;
    private Coroutine coBehavior;
    public eCommanderState currentState;
    public EffectEvents effectEvents;
    public Action OnTargetFound;
    public Action OnAttackComplete;
    public Action OnIdleComplete;
    public enum eCommanderState { SEARCH, ATTACK, IDLE, USESKILL, RETURN }
    private void Start()
    {

    }
    private void DisplayState()
    {
        //Debug.Log(this.currentState.ToString());
    }
    public void Init(CommanderData data, List<TestMonster> listMonsters)
    {
        //가져온 데이터를 참조합니다
        this.data = data;
        this.calculatedDamage = FormulaCalculator.GetInstance().AllCalculatedDamage_Commander(data.id);
        this.calculatedAspd = FormulaCalculator.GetInstance().AllCalculatedAspd_Commander(data.id);

        if (this.animator == null)
        {
            this.animator = this.gameObject.GetComponentInChildren<Animator>();
        }

        //effectEvents를 초기화합니다
        this.effectEvents = this.GetComponentInChildren<EffectEvents>();
        //Debug.Log("============================");

        //Debug.Log(effectEvents);

        //boxCollider를 붙입니다(Hero Merge를 위함)
        this.boxCollider = this.gameObject.AddComponent<BoxCollider>();
        this.boxCollider.size = new Vector3(1, 3, 1);
        this.boxCollider.center = new Vector3(0, 1.2f, 0);
        this.boxCollider.isTrigger = true;

        //몬스터 리스트 참조를 가져옵니다
        this.listMonsters = listMonsters;

        this.ChangeBehavior(eCommanderState.SEARCH);
    }

    #region CHANGEBEHAVIOR
    public void ChangeBehavior(eCommanderState state)
    {

        switch (state)
        {
            case eCommanderState.SEARCH:
                {
                    Search();
                }
                break;
            case eCommanderState.ATTACK:
                {
                    Attack();
                }
                break;
            case eCommanderState.IDLE:
                {
                    Idle();
                }
                break;
            case eCommanderState.USESKILL:
                {
                    UseSkill();
                }
                break;
            case eCommanderState.RETURN:
                {
                    Return();
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
        currentState = eCommanderState.SEARCH;
        DisplayState();
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
            ChangeBehavior(eCommanderState.IDLE);
        }

        var layerMask = 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Obstacle");
        while (true)
        {
            this.animator.Play("Search", -1, 0);

            if (listMonsters.Count != 0)
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
                        //Debug.Log(targetedMonster.name);

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
        currentState = eCommanderState.SEARCH;
        DisplayState();
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
            //Debug.Log($"targetedMonster: {targetedMonster}");
            this.gameObject.transform.root.LookAt(monster.transform);

            if (effectEvents != null)
            {
                effectEvents.monster = monster.gameObject;
            }
            this.animator.speed = 1.5f;
            this.animator.Play($"{data.prefab_name}_Attack", -1, 0);

            var runtimeAC = this.animator.runtimeAnimatorController;
            for (int i = 0; i < runtimeAC.animationClips.Length; i++)
            {
                Debug.Log($"{data.prefab_name}----------{runtimeAC.animationClips[i].name}");
                //Debug.Log(runtimeAC.animationClips[0].name);
                if (runtimeAC.animationClips[i].name == $"{data.prefab_name}_Attack")
                {
                    time = runtimeAC.animationClips[i].length;
                    break;
                }
            }
            Debug.Log($"Commander_AttackTime:{time}");
            yield return new WaitForSeconds(time);
            OnAttackComplete();
            break;
        }
    }
    #endregion

    #region IDLE
    public void Idle()
    {
        currentState = eCommanderState.SEARCH;
        DisplayState();
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(IdleImpl());
    }
    IEnumerator IdleImpl()
    {
        targetedMonster = null;
        var time = 0.5f;
        //Debug.Log("IdleImpl");
        while (true)
        {
            this.animator.Play($"{data.prefab_name}_Idle");
            //Debug.Log("아이들링");
            //var runtimeAC = this.animator.runtimeAnimatorController;
            //for (int i = 0; i < runtimeAC.animationClips.Length; i++)
            //{
            //    //Debug.Log($"{data.prefab_name}----------{runtimeAC.animationClips[i].name}");
            //    //Debug.Log(runtimeAC.animationClips[0].name);
            //    if (runtimeAC.animationClips[i].name == $"{data.prefab_name}_Idle")
            //    {
            //        time = runtimeAC.animationClips[i].length;
            //        break;
            //    }
            //}
            //Debug.Log($"Commander_IdleTime:{time}");
            yield return new WaitForSeconds(calculatedAspd);
            OnIdleComplete();
            break;
        }

    }
    #endregion

    #region RETURN
    private void Return()
    {
        throw new NotImplementedException();
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

    public void SetListMonsters(List<TestMonster> listMonsters)
    {
        this.listMonsters = listMonsters;
    }
}
