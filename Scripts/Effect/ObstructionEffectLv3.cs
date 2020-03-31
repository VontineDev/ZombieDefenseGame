using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MarchingBytes;

public class ObstructionEffectLv3 : EffectEvents
{
    public void StunAttack()
    {
        var prefab = (GameObject)(from obj in App.instance.resource
                                  where obj.name == "ObstructionEffect"
                                  select obj).FirstOrDefault();

        EasyObjectPool.instance.MakePoolInfo("ObstructionEffect", prefab);

        GameObject go = EasyObjectPool.instance.GetObjectFromPool("ObstructionEffect", monster.transform.position, Quaternion.identity);

        stunAttackAction(monster);
    }
}
