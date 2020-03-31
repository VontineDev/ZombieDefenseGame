using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderEffect : EffectEvents
{

    public void ThunderShoot()
    {
        var findGo = TransformDeepChildExtension.FindDeepChild(this.transform, "thunderPos").gameObject;
        var effectGo = Resources.Load<GameObject>("Prefab/Effect/Thunder");
        var effect = Instantiate(effectGo);
        effect.transform.position = findGo.transform.position;
        var thunderThrowing = effect.AddComponent<ThunderThrowing>();

        thunderThrowing.ThunderThrow(effect, monster);
        thunderThrowing.throwAction = (targetMonster) =>
        {
            rangeMonstersAction(targetMonster);
        };


    }
}
