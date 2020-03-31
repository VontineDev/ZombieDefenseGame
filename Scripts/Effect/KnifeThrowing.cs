using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeThrowing : MonoBehaviour
{
    private List<GameObject> findMonster = new List<GameObject>();
    public System.Action<List<GameObject>> throwAction;
    int speed = 10;
    public void KnifeThrow(GameObject knife, GameObject taget)
    {
        StartCoroutine(MoveThrow(knife, taget));
    }
    IEnumerator MoveThrow(GameObject knife, GameObject taget)
    {
        while (true)
        {
            if (taget != null)
            {
                //knife.transform.position += Vector3.forward * Time.deltaTime * 10;
                var targetPos = taget.transform.position;
                targetPos.y++;
                this.transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);                
                knife.transform.Rotate(Vector3.right * 10);
                knife.transform.Rotate(Vector3.up * 10);
                var knifePos = knife.transform.position;
                var dis = Vector3.Distance(knifePos, targetPos);
                if (dis < 0.2)
                {
                    Destroy(knife);
                    findMonster.Add(taget);
                    throwAction(findMonster);
                    break;
                }
                yield return null;
            }
            
        }
    }
}
