using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteEffect2 : EffectEvents
{
    public void DynamateShoot()
    {
        var findWeapon = TransformDeepChildExtension.FindDeepChild(this.transform, "SA_Wep_DynamiteBundle").gameObject;
        findWeapon.SetActive(false);
        var findGo = TransformDeepChildExtension.FindDeepChild(this.transform, "DanamatePos").gameObject;
        var effectGo = Resources.Load<GameObject>("Prefab/Effect/Dynamate");
        var effect = Instantiate(effectGo);
        effect.transform.position = findGo.transform.position;
        var DanamateThrowing = effect.AddComponent<DynamiteThrowing2>();
        if (monster != null)
        {
            //DanamateThrowing.DanamateThrow(effect, monster);
            DanamateThrowing.Shoot(effect, findGo.transform.position, monster, 9, 5);
        }

        DanamateThrowing.dynamiteAction = (targetMonsters) => {
            //Debug.Log("rangeMonstersAction");

            rangeMonstersAction(targetMonsters);
        };
    }
}
