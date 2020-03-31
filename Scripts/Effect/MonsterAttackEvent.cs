using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackEvent : MonoBehaviour
{
    public System.Action monsterAttackAction;
    
    public void FenceAttack()
    {
        this.monsterAttackAction();
    }
    
}
