using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderThrowing : MonoBehaviour
{
    private List<GameObject> findMonster = new List<GameObject>();
    public System.Action<List<GameObject>> throwAction;
    int speed = 15;
    public void ThunderThrow(GameObject thunder, GameObject taget)
    {
        Debug.Log(thunder + "--------------------------");
        Debug.Log(taget + "--------------------------");
        StartCoroutine(MoveThrow(thunder, taget));
    }
    IEnumerator MoveThrow(GameObject thunder, GameObject taget)
    {

        while (true)
        {
            if (taget != null)
            {
                //knife.transform.position += Vector3.forward * Time.deltaTime * 10;
                var targetPos = taget.transform.position;
                targetPos.y++;
                var changePos = targetPos;
                thunder.transform.position = Vector3.MoveTowards(transform.position, changePos, speed * Time.deltaTime);
                var thunderPos = thunder.transform.position;
                thunder.transform.LookAt(taget.transform);
                var dis = Vector3.Distance(thunderPos, targetPos);
                //Debug.Log($"dis : {dis}");
                if (dis < 1.5)
                {
                    var findTagMonster = GameObject.FindGameObjectsWithTag("Monster");
                    foreach (var monsters in findTagMonster)
                    {
                        var distance = Vector3.Distance(thunder.transform.position, monsters.transform.position);
                        if (distance < 3)
                        {
                            findMonster.Add(monsters);
                        }

                    }
                    Debug.Log("몬스터가 맞았어");
                    Destroy(thunder);

                    throwAction(findMonster);
                    break;
                }
                yield return null;
            }

        }
    }
}
