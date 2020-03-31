using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class TestFlamethrower : MonoBehaviour
{
    public Animator anim;
    public UIButton btnAttack;
    public EffectEvents effectEvents;
    public TestMonster testMonster;
    public Material deMaterial;
    public TestFence fence;
    // Start is called before the first frame update
    void Start()
    {
        var json = Resources.Load<TextAsset>("Data/defense_monster_data").text;
        var arr = JsonConvert.DeserializeObject<DefenseMonsterData[]>(json);

        testMonster.Init(Vector3.zero, arr[0], fence, deMaterial);
        testMonster.hp = 50000;
        this.anim = this.gameObject.GetComponentInChildren<Animator>();

        this.effectEvents = this.GetComponentInChildren<EffectEvents>();
        this.effectEvents.rangeMonstersAction = (listMonsters) =>
        {
            for (int i = 0; i < listMonsters.Count; i++)
            {
                int capt = i;

                listMonsters[capt].gameObject.GetComponent<TestMonster>().Hit(2);
                Debug.Log($"맞냐? {listMonsters.Count} ");

            }
        };
        this.btnAttack.onClick.Add(new EventDelegate(() =>
        {
            Attack();

        }));
    }
    private void Attack()
    {
        StartCoroutine(AttackImpl());
    }

    IEnumerator AttackImpl()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            this.anim.Play("Comm01_Attack", -1, 0);
        }
    }
}
