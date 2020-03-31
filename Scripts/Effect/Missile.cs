using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject missile;
    public int speed = 15;

    private void Start()
    {
        this.missile = this.gameObject;
        MissileMove();
    }
    public void MissileMove()
    {
        StartCoroutine(MissileMoveCor());

    }
    IEnumerator MissileMoveCor()
    {
        while (true)
        {
            missile.transform.position += Vector3.down * Time.deltaTime * speed;
            if(missile.transform.position.y < 1.3)
            {
                //----------폭발 이펙트 들어가야됨!!----------
                LoadEffect();
                
                Destroy(this.gameObject);
                break;
            }
            yield return null;
        }
    }
    public GameObject LoadEffect()
    {
        var missileBoom = Resources.Load<GameObject>("Prefab/Effect/MissileBoom");
        var missileEffect = Instantiate(missileBoom);
        missileEffect.transform.position = missile.transform.position;
        return missileEffect;
    }
}
