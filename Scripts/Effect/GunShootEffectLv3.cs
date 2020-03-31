using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootEffectLv3 : EffectEvents
{
    public void GunShoot()
    {
        var findGo = TransformDeepChildExtension.FindDeepChild(this.transform, "shootPoint").gameObject;
        //Debug.Log(findGo.name);
        var effectGo = Resources.Load<GameObject>("Prefab/Effect/GunEffect");
        var effect = Instantiate(effectGo);
        effect.transform.position = findGo.transform.position;
        var bulletGo = Resources.Load<GameObject>("Prefab/Effect/Bullet1");
        var bullet = Instantiate(bulletGo);
        bullet.transform.position = findGo.transform.position;
        var gunShootThrowing = bullet.AddComponent<GunShootThrowing>();
        gunShootThrowing.GunThrow(bullet, monster);
        SoundEffectManager.instance.PlayAnimAudio(1);
        gunShootThrowing.throwAction = (targetMonster) =>
        {
            if (findGo != null)
            {
                foreach (var mon in targetMonster)
                {
                    //Debug.Log($"mon: {mon}");
                }

                HpPercentDmgAction(targetMonster);
            }
        };
    }
}
