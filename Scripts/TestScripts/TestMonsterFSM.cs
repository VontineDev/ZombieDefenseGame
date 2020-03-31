using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonsterFSM : MonoBehaviour
{
    public enum eTestMonsterState { IDLE,MOVE,ATTACK}
    public Animator animator;
    private Coroutine coBehavior;
    public void Init()
    {
        this.animator = this.gameObject.GetComponentInChildren<Animator>();
        Idle();
    }
    public void ChangeBehavior(eTestMonsterState state)
    {
        switch (state)
        {
            case eTestMonsterState.IDLE:
                {
                    Idle();
                }
                break;
            case eTestMonsterState.MOVE:
                {
                    Move();
                }
                break;
            case eTestMonsterState.ATTACK:
                {
                    Attack();
                }
                break;
            default:
                break;
        }
    }
    private void Idle()
    {
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(IdleImpl());
    }
    private void Move()
    {
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(MoveImpl());

    }
    private void Attack()
    {
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(AttackImpl());
    }
    IEnumerator IdleImpl()
    {
        while (true)
        {
            this.animator.Play("Monster_Idle");
            float time = 1f;
            var runtimeAC = this.animator.runtimeAnimatorController;
            for (int i = 0; i < runtimeAC.animationClips.Length; i++)
            {
                Debug.Log($"----------{runtimeAC.animationClips[i].name}");
                Debug.Log(runtimeAC.animationClips[0].name);
                if (runtimeAC.animationClips[i].name == $"mixamo.com")
                {
                    time = runtimeAC.animationClips[i].length;
                    Debug.Log(time);
                }
            }
            yield return new WaitForSeconds(time);

        }
    }
    IEnumerator MoveImpl()
    {
        while (true)
        {
            yield return null;
            this.animator.Play("Monster_Run", -1, 0);
            float time = 1f;
            var runtimeAC = this.animator.runtimeAnimatorController;
            for (int i = 0; i < runtimeAC.animationClips.Length; i++)
            {
                Debug.Log($"----------{runtimeAC.animationClips[i].name}");
                Debug.Log(runtimeAC.animationClips[0].name);
                if (runtimeAC.animationClips[i].name == $"mixamo.com")
                {
                    time = runtimeAC.animationClips[i].length;
                    Debug.Log(time);
                }
            }
            yield return new WaitForSeconds(time);
        }
    }
    IEnumerator AttackImpl()
    {
        while (true)
        {
            //yield return null;
            this.animator.Play("Monster_TwoAttack", -1, 0);
            float time = 1f;
            var runtimeAC = this.animator.runtimeAnimatorController;
            for (int i = 0; i < runtimeAC.animationClips.Length; i++)
            {
                Debug.Log($"----------{runtimeAC.animationClips[i].name}");
                Debug.Log(runtimeAC.animationClips[0].name);
                if (runtimeAC.animationClips[i].name == $"mixamo.com")
                {
                    time = runtimeAC.animationClips[i].length;
                    Debug.Log(time);
                }
            }
            yield return new WaitForSeconds(time);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
