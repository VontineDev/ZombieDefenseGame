using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class JetMove : EffectEvents
{
    public GameObject jet;
    public GameObject startpos1;
    public GameObject startpos2;
    public GameObject startpos3;
    public GameObject missile;
    public int speed = 20;

    
    void Start()
    {
        
        
        
    }   
    public void JetSkill()
    {
        StartCoroutine(JetSkillCor());
    }
    IEnumerator JetSkillCor()
    {
        this.jet = this.gameObject;
        MissileStart();
        while (true)
        {
            jet.transform.position += Vector3.forward * Time.deltaTime * speed;
            
            if (jet.transform.position.z > 25)
            {
                jetcollAction();
                EasyObjectPool.instance.ReturnObjectToPool(this.jet);
                break;
            }
            yield return null;
        }       
    }
    public void MissileStart()
    {
        StartCoroutine(MissileStartCor());
    }
    IEnumerator MissileStartCor()
    {
        while (true)
        {
            if (jet.transform.position.z > -5)
            {
                this.missile = LoadMissile();
                var missile1 = LoadMissile();
                var missile2 = LoadMissile();
                missile.transform.position = startpos1.transform.position;
                missile1.transform.position = startpos2.transform.position;
                missile2.transform.position = startpos3.transform.position;               
            }
            if (jet.transform.position.z > 13)
            {
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    //나중에는 오브젝트 풀에서 꺼내와야됨
    public GameObject LoadMissile()
    {
        var missile = Resources.Load<GameObject>("Prefab/Effect/Missile");
        var missileGo = Instantiate(missile);
        return missileGo;
        
    }
}
