using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectEvents : MonoBehaviour
{
    public GameObject monster;
    public System.Action<List<GameObject>> rangeMonstersAction;
    public Action<List<GameObject>> HpPercentDmgAction;
    public Action<List<GameObject>> attackSpeedUpAction;
    public Action jetcollAction;
    public System.Action<List<GameObject>> dotDmgAction;
    public System.Action<List<GameObject>> dotDmgActionLv2;
    public System.Action<List<GameObject>> dotDmgActionLv3;

    public System.Action<GameObject> knockBackAttackAction;
    public System.Action<GameObject> slowAttackAction;
    public System.Action<GameObject> stunAttackAction;

    public System.Action<int> aspdUpAction;
}
