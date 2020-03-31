using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAtttackAction : MonoBehaviour
{
    public System.Action<GameObject,float> monsterAttackAction;
    public float dmg;
    public void FenceAttackAction()
    {
        this.monsterAttackAction(this.gameObject.transform.root.gameObject,this.dmg);
    }
    public void Init(float dmg)
    {
        this.dmg = dmg;
    }
}
