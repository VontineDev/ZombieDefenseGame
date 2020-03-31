using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowEffect : EffectEvents
{
    private GameObject findGo;
    public static float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }
    public void Flamethrow()
    {
        findGo = TransformDeepChildExtension.FindDeepChild(this.transform, "firePos").gameObject;
        var directionVector = this.transform.root.transform.rotation;
        var effectGo = Resources.Load<GameObject>("Prefab/Effect/FlameThrower");
        var effect = Instantiate(effectGo);
        effect.transform.position = findGo.transform.position;       
        effect.transform.rotation *= directionVector;   
        var pos = findGo.transform.position;
        FlameRay(pos);
    }

    private void FlameRay(Vector3 pos)
    {
        List<GameObject> listMonsters = new List<GameObject>();

        var characterAngle = this.transform.root.rotation.eulerAngles;

        GameObject charcterDirectionVector = new GameObject();

        charcterDirectionVector.transform.position = new Vector3(Mathf.Sin(characterAngle.y * Mathf.PI / 180), 0, this.transform.root.position.z + Mathf.Abs(Mathf.Cos(characterAngle.y * Mathf.PI / 180)));

        var data = this.transform.root.GetComponent<TestCommander>().data;

        var findTagMonster = GameObject.FindGameObjectsWithTag("Monster");

        foreach (var monster in findTagMonster)
        {
            var angle = Mathf.Abs(Vector3.Angle(monster.transform.position, charcterDirectionVector.transform.position));
            var dis = Vector3.Distance(findGo.transform.position, monster.transform.position);

            if (dis <= data.atkRange)
            {
                if (angle <= 30)
                {
                    listMonsters.Add(monster);
                }
            }
        }

        foreach (var mon in listMonsters)
        {
            Debug.Log(mon.name);
        }
        this.dotDmgAction(listMonsters);
    }

}
