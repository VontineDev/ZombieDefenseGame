using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MarchingBytes;

public class ObstructionEffectLv1 : EffectEvents
{
    public void KnockBackAttack()
    {
        var prefab = (GameObject)(from obj in App.instance.resource
                                  where obj.name == "ObstructionEffect"
                                  select obj).FirstOrDefault();

        EasyObjectPool.instance.MakePoolInfo("ObstructionEffect", prefab);

        GameObject go = EasyObjectPool.instance.GetObjectFromPool("ObstructionEffect", monster.transform.position, Quaternion.identity);

        knockBackAttackAction(monster);
        SoundEffectManager.instance.PlayAnimAudio(10);
    }
}
