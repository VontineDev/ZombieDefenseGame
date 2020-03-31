using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteThrowing2 : MonoBehaviour
{
    private List<GameObject> findMonster = new List<GameObject>();
    public System.Action<List<GameObject>> dynamiteAction;
    private GameObject bullet;
    private float tx;

    private float ty;

    private float tz;

    private float v;

    public float g = 9.8f;

    private float elapsed_time;

    public float max_height;

    private float t;

    private Vector3 start_pos;

    private Vector3 end_pos;
    private float dat;  //도착점 도달 시간 

    //public void DanamateThrow(GameObject dynamite, GameObject taget)
    //{
    //    //StartCoroutine(MoveThrow(dynamite, taget));
    //}
    //IEnumerator MoveThrow(GameObject dynamite, GameObject taget)
    //{
    //    while (true)
    //    {
    //        if (taget != null)
    //        {
    //            //knife.transform.position += Vector3.forward * Time.deltaTime * 10;
    //            var targetPos = taget.transform.position;
    //            targetPos.y++;
    //            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    //            dynamite.transform.Rotate(Vector3.right * 10);
    //            dynamite.transform.Rotate(Vector3.up * 10);
    //            var dynamitePos = dynamite.transform.position;
    //            var dis = Vector3.Distance(dynamitePos, targetPos);
    //            if (dis < 0.2)
    //            {
    //                Destroy(dynamite);
    //                RoadBoom(targetPos);
    //                //다이나마이트가 도착하는 시점에 폭발 이펙트 로드해서 보여주기

    //                if (taget != null)
    //                {
    //                    taget.GetComponent<TestMonster>().KnockBack();
    //                    dynamiteAction(taget);
    //                }

    //                break;
    //            }
    //            yield return null;
    //        }

    //    }
    //}
    public void Shoot(GameObject bullet, Vector3 startPos, GameObject taget, float g, float max_height)
    {
        start_pos = startPos;
        end_pos = taget.transform.position;
        this.g = g;
        this.max_height = max_height;
        this.bullet = bullet;
        this.bullet.transform.position = start_pos;
        var dh = end_pos.y - startPos.y;
        var mh = max_height - startPos.y;
        ty = Mathf.Sqrt(2 * this.g * mh);
        float a = this.g;
        float b = -2 * ty;
        float c = 2 * dh;
        dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
        tx = -(startPos.x - end_pos.x) / dat;
        tz = -(startPos.z - end_pos.z) / dat;
        this.elapsed_time = 0;
        StartCoroutine(this.ShootImpl(bullet, taget));
    }
    IEnumerator ShootImpl(GameObject dynamite, GameObject monster)
    {
        while (true)
        {
            this.elapsed_time += Time.deltaTime;
            var tx = start_pos.x + this.tx * elapsed_time;
            var ty = start_pos.y + this.ty * elapsed_time - 0.5f * g * elapsed_time * elapsed_time;
            var tz = start_pos.z + this.tz * elapsed_time;
            var tpos = new Vector3(tx, ty, tz);
            bullet.transform.LookAt(tpos);
            bullet.transform.position = tpos;
            if (this.elapsed_time >= this.dat)
            {
                var findTagMonster = GameObject.FindGameObjectsWithTag("Monster");
                foreach (var aa in findTagMonster)
                {
                    var dis = Vector3.Distance(dynamite.transform.position, aa.transform.position);
                    if (dis < 6)
                    {
                        findMonster.Add(aa);
                    }
                }
                var targetPos = dynamite.transform.position;
                Destroy(dynamite);
                SoundEffectManager.instance.PlayAnimAudio(0);
                RoadBoom(targetPos);
                //다이나마이트가 도착하는 시점에 폭발 이펙트 로드해서 보여주기

                if (monster != null)
                {
                    dynamiteAction(findMonster);
                }
                break;
            }
            yield return null;
        }
    }
    void RoadBoom(Vector3 taget)//폭발 이펙트
    {
        var boomGo = Resources.Load<GameObject>("Prefab/Effect/Boom2");
        var boom = Instantiate(boomGo);
        boom.transform.position = taget;
    }
}
