using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGunThrowing : MonoBehaviour
{
    private List<GameObject> findMonster = new List<GameObject>();
    public System.Action<List<GameObject>> minithrowAction;
    int speed = 30;
    public void MiniGunThrow(GameObject gunBullet, GameObject taget)
    {
        StartCoroutine(MoveThrow(gunBullet, taget));
    }
    IEnumerator MoveThrow(GameObject gunBullet, GameObject taget)
    {
        while (true)
        {
            if (taget != null)
            {
                
                //knife.transform.position += Vector3.forward * Time.deltaTime * 10;
                var targetPos = taget.transform.position;
                targetPos.y++;
                var changePos = targetPos;
                gunBullet.transform.position = Vector3.MoveTowards(transform.position, changePos, speed * Time.deltaTime);
                var gunBulletPos = gunBullet.transform.position;
                gunBullet.transform.LookAt(taget.transform);
                var dis = Vector3.Distance(gunBulletPos, targetPos);
                //Debug.Log($"dis : {dis}");
                if (dis < 1.5)
                {
                    //Debug.Log("몬스터가 맞았어");
                    Destroy(gunBullet);
                    findMonster.Add(taget);
                    if (this.transform.root.gameObject != null)
                    {
                        minithrowAction(findMonster);
                    }
                    break;
                }
                yield return null;
            }

        }
    }
}
