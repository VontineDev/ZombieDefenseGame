using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGunShootEffect : EffectEvents
{
    public void MiniGunShoot()
    {
        var findGo = TransformDeepChildExtension.FindDeepChild(this.transform, "minigunShootPoint").gameObject;
        //Debug.Log(findGo.name);
        var effectGo = Resources.Load<GameObject>("Prefab/Effect/MinigunEffect");
        var effect = Instantiate(effectGo);
        effect.transform.position = findGo.transform.position;
        var bulletGo = Resources.Load<GameObject>("Prefab/Effect/Bullet1");
        var bullet = Instantiate(bulletGo);
        bullet.transform.position = findGo.transform.position;
        var gunShootThrowing = bullet.AddComponent<MiniGunThrowing>();
        gunShootThrowing.MiniGunThrow(bullet, monster);
        SoundEffectManager.instance.PlayAnimAudio(3);
        gunShootThrowing.minithrowAction = (targetMonster) =>
        {
            if (findGo != null)
            {
                rangeMonstersAction(targetMonster);
            }
        };
    }
}
