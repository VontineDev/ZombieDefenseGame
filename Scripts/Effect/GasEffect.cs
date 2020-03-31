using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasEffect : EffectEvents
{
    public void GasShoot()
    {
        var findWeapon = TransformDeepChildExtension.FindDeepChild(this.transform, "SA_Wep_Grenade").gameObject;
        findWeapon.SetActive(false);
        var findGo = TransformDeepChildExtension.FindDeepChild(this.transform, "GasPos").gameObject;
        var effectGo = Resources.Load<GameObject>("Prefab/Effect/GasBoomPrefab");
        var effect = Instantiate(effectGo);
        effect.transform.position = findGo.transform.position;
        var GasThrowing = effect.AddComponent<GasThrowing>();
        if (monster != null)
        {
            //DanamateThrowing.DanamateThrow(effect, monster);
            GasThrowing.Shoot(effect, findGo.transform.position, monster, 9, 5);
        }

        GasThrowing.gasAction = (targetMonsters) => {
            dotDmgAction(targetMonsters);
            SoundEffectManager.instance.PlayAnimAudio(9);
        };
    }
}
