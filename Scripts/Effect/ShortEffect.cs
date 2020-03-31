using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortEffect : EffectEvents
{
    public void ShortShoot1()
    {
        var findGo = TransformDeepChildExtension.FindDeepChild(this.transform, "ShortPoint1").gameObject;       
        var effectGo = Resources.Load<GameObject>("Prefab/Effect/shortGunEffect");
        var effect = Instantiate(effectGo);
        effect.transform.position = findGo.transform.position;
        var bulletGo = Resources.Load<GameObject>("Prefab/Effect/Bullet1");
        var bullet = Instantiate(bulletGo);
        bullet.transform.position = findGo.transform.position;
        var gunShootThrowing = bullet.AddComponent<ShortThrowing>();
        gunShootThrowing.GunThrow(bullet, monster);
        SoundEffectManager.instance.PlayAnimAudio(2);
        gunShootThrowing.shortGunAction = (targetMonster) => {
            if(targetMonster != null)
            {
                rangeMonstersAction(targetMonster);
            }
            
        };
    }
    public void ShortShoot2()
    {
        var findGo = TransformDeepChildExtension.FindDeepChild(this.transform, "ShortPoint2").gameObject;
        var effectGo = Resources.Load<GameObject>("Prefab/Effect/shortGunEffect");
        var effect = Instantiate(effectGo);
        effect.transform.position = findGo.transform.position;
        var bulletGo = Resources.Load<GameObject>("Prefab/Effect/Bullet1");
        var bullet = Instantiate(bulletGo);
        bullet.transform.position = findGo.transform.position;
        var gunShootThrowing = bullet.AddComponent<ShortThrowing>();
        gunShootThrowing.GunThrow(bullet, monster);
        SoundEffectManager.instance.PlayAnimAudio(2);
        gunShootThrowing.shortGunAction = (targetMonster) => {
            if (targetMonster != null)
            {
                rangeMonstersAction(targetMonster);
            }

        };
    }
}
