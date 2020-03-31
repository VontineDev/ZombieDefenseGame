using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFence : MonoBehaviour
{
    public FenceData data;
    public float hp;
    public System.Action<GameObject> OnMonsterRange;
    public System.Action<float> OnChangeFenceHp;
    private void OnTriggerEnter(Collider other)
    {
        OnMonsterRange(other.gameObject);
    }
    public void Init(FenceData data)
    {
        this.data = data;
        this.hp = this.data.maxHp;
    }
    public void Hit(float damage)
    {
        this.hp -= damage;
        float percentage = hp / this.data.maxHp;
        Debug.Log($"percentage:{percentage} hp:{hp} maxHp:{this.data.maxHp}");
        OnChangeFenceHp(percentage);
    }
}
