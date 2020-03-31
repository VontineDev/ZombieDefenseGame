using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FormulaCalculator
{

    private static FormulaCalculator Instance;
    public static FormulaCalculator GetInstance()
    {
        if (FormulaCalculator.Instance == null)
        {
            FormulaCalculator.Instance = new FormulaCalculator();
        }
        return FormulaCalculator.Instance;
    }

    public float CalculateDamage(float damage, int lv)
    {
        return damage + damage * lv;
    }
    public float CalculateAspd(float Aspd, int lv)
    {
        return Aspd * (100 - lv) / 100;
    }

    public float AllCalculatedDamage_Commander(int id)
    {
        var comm_data = DataManager.GetInstance().dicCommanderData[id];

        var comm_info = (from info in DataManager.GetInstance().dicCommanderinfo
                         where info.Value.commander_id == id
                         select info.Value).First();
        var equipped_itemId = (from info in DataManager.GetInstance().dicItemInfo
                               where info.Value.isEquipped == true && info.Value.type==1
                               select info.Key).First();
        var item_data = DataManager.GetInstance().dicItemData[equipped_itemId];
        var total = comm_data.damage + comm_data.damage * comm_info.damageLv + item_data.damage;

        return total;
    }
    public float AllCalculatedAspd_Commander(int id)
    {
        var comm_data = DataManager.GetInstance().dicCommanderData[id];

        var comm_info = (from info in DataManager.GetInstance().dicCommanderinfo
                         where info.Value.commander_id == id
                         select info.Value).First();
        var equipped_itemId = (from info in DataManager.GetInstance().dicItemInfo
                               where info.Value.isEquipped == true && info.Value.type==2
                               select info.Key).First();
        var item_data = DataManager.GetInstance().dicItemData[equipped_itemId];
        var total = comm_data.atkSpeed * ((100 - comm_info.atkSpeedLV) / 100) * ((100 - item_data.atkSpeed) / 100);
        return total;
    }
    public float AllCalculatedDamage_Hero(int id)
    {
        var hero_data = DataManager.GetInstance().dicHeroData[id];

        var user_info = DataManager.GetInstance().userInfo;      

        var total = hero_data.damage + hero_data.damage * user_info.heroDamageLv;

        return total;
    }
    public float AllCalculatedAspd_Hero(int id)
    {
        var hero_data = DataManager.GetInstance().dicHeroData[id];

        var user_info = DataManager.GetInstance().userInfo;
       
        var total = hero_data.atkSpeed * ((100 - user_info.heroAtkSpeedLv) / 100);

        return total;
    }


    public float CalculateIdleTime(float idleTime, int bySelfDecreaedDelay, int byOtherDecreasedDelay)
    {
        return idleTime * (100 - bySelfDecreaedDelay) / 100 * (100 - byOtherDecreasedDelay) / 100;
    }

    public float CalculateDecreasedMoveSpeed(float moveSpeed, float percentage)
    {
        return moveSpeed * (100 - percentage) / 100;
    }

}