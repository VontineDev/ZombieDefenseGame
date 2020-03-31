using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TestFSM : MonoBehaviour
{
    public TestMonsterFSM monster;
    public UIButton btnIdle;
    public UIButton btnMove;
    public UIButton btnAttack;
    private void Start()
    {
        monster.Init();
        this.btnIdle.onClick.Add(new EventDelegate(() =>
        {
            monster.ChangeBehavior(TestMonsterFSM.eTestMonsterState.IDLE);
        }));

        this.btnMove.onClick.Add(new EventDelegate(() =>
        {
            monster.ChangeBehavior(TestMonsterFSM.eTestMonsterState.MOVE);

        }));

        this.btnAttack.onClick.Add(new EventDelegate(() =>
        {
            monster.ChangeBehavior(TestMonsterFSM.eTestMonsterState.ATTACK);
        }));

    }
}
