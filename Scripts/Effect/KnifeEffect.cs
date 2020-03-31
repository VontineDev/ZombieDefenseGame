using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeEffect : EffectEvents
{
   
    public void KnifeShoot()
    {
        //TestCommander.OnAttack = (monster) => { 
        //    this.monster = monster; 
        //};
        
        var findGo = TransformDeepChildExtension.FindDeepChild(this.transform, "KnifePoint").gameObject;
        var effectGo = Resources.Load<GameObject>("Prefab/Effect/Knife");
        var effect = Instantiate(effectGo);       
        effect.transform.position = findGo.transform.position;
        var knifeThrowing = effect.AddComponent<KnifeThrowing>();
        knifeThrowing.KnifeThrow(effect, monster);
        SoundEffectManager.instance.PlayAnimAudio(4);
        knifeThrowing.throwAction = (targetMonster) => {
            rangeMonstersAction(targetMonster);           
        };
    }
    
}
