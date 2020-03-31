using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UIPopup_MyInfo : MonoBehaviour
{
    public UIButton btnClose;

    //커맨더 오브젝트, 초기위치
    public GameObject commanderIconPos;
    public GameObject commanderGo;
    public Vector3 commaderPosition = new Vector3(0, 0, 0);
    public Vector3 commanderScale = new Vector3(150, 150, 150);
    public Action onChangeMoney;

    //Commander
    private int presentComm_id;

    public UILabel lbCommanderDamage;
    public UILabel lbCommanderAspd;
    public UILabel lbCommanderDmGold;
    public UILabel lbCommanderAsGold;

    public UILabel lbCommanderDamageLV;
    public UILabel lbCommanderAspdLV;

    public UIButton btnUpgradeCommanderDamageLV;
    public UIButton btnUpgradeCommanderAspdLV;

    //Hero
    public UILabel lbHeroDamageLV;
    public UILabel lbHeroAspdLV;
    public UILabel lbHeroDmGold;
    public UILabel lbHeroAsGold;

    public UIButton btnUpgradeHeroDamageLV;
    public UIButton btnUpgradeHeroAspdLV;

    public UIButton btnHeroStatus;
    public GameObject ui_HeroStatus;
    public void Init()
    {
        DisplayInfos();

        btnClose.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            this.gameObject.SetActive(false);
            DataManager.GetInstance().SaveCommanderInfo();
        }));

        btnHeroStatus.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            ui_HeroStatus.gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }));
        this.btnUpgradeCommanderDamageLV.onClick.Add(new EventDelegate(() =>
        {
            var damageLv = DataManager.GetInstance().dicCommanderinfo[presentComm_id].damageLv;
            if (damageLv < 30)
            {
                if (DataManager.GetInstance().userInfo.cash >= DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().dicCommanderinfo[presentComm_id].damageLv + 1].gold)
                {
                    DataManager.GetInstance().userInfo.cash -= (ulong)DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().dicCommanderinfo[presentComm_id].damageLv + 1].gold;
                    onChangeMoney();
                    if (DataManager.GetInstance().dicCommanderinfo[presentComm_id].damageLv < 30)
                    {
                        SoundEffectManager.effectUpgradeAction();
                        Debug.Log($"커맨더데미지업");
                        DataManager.GetInstance().dicCommanderinfo[presentComm_id].damageLv++;
                    }
                    else if (DataManager.GetInstance().dicCommanderinfo[presentComm_id].damageLv == 30)
                    {
                        Debug.Log("업그레이드 완료");
                    }
                    DataManager.GetInstance().SetInfoFromLv();
                    DisplayCommanderInfo();
                }
                else
                {
                    Debug.Log("돈없쪙");
                }
            }
            else
            {
                Debug.Log("CommanderDamageLV 30-------MAX");
            }
        }));

        this.btnUpgradeCommanderAspdLV.onClick.Add(new EventDelegate(() =>
        {
            var speedLv = DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().dicCommanderinfo[presentComm_id].id].level;
            if (speedLv < 30)
            {
                if (DataManager.GetInstance().userInfo.cash >= DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().dicCommanderinfo[presentComm_id].atkSpeedLV + 1].gold)
                {
                    DataManager.GetInstance().userInfo.cash -= (ulong)DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().dicCommanderinfo[presentComm_id].atkSpeedLV + 1].gold;
                    onChangeMoney();
                    if (DataManager.GetInstance().dicCommanderinfo[presentComm_id].atkSpeedLV < 30)
                    {

                        SoundEffectManager.effectUpgradeAction();
                        Debug.Log($"커맨더공속업");
                        DataManager.GetInstance().dicCommanderinfo[presentComm_id].atkSpeedLV++;
                    }
                    else if (DataManager.GetInstance().dicCommanderinfo[presentComm_id].atkSpeedLV == 30)
                    {
                        Debug.Log("업그레이드 완료");
                    }
                    DataManager.GetInstance().SetInfoFromLv();
                    DisplayCommanderInfo();
                }
                else
                {
                    Debug.Log("돈없쪙");
                }
            }
            else
            {

                Debug.Log("CommanderAttackSpeedLV 30-------MAX");
            }
        }));
        this.btnUpgradeHeroDamageLV.onClick.Add(new EventDelegate(() =>
        {
            var heroDamageLv = DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().userInfo.heroDamageLv + 1].level;
            if (heroDamageLv <= 30)
            {
                if (DataManager.GetInstance().userInfo.cash >= DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().userInfo.heroDamageLv + 1].gold)
                {
                    DataManager.GetInstance().userInfo.cash -= (ulong)DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().userInfo.heroDamageLv + 1].gold;
                    onChangeMoney();
                    if (DataManager.GetInstance().userInfo.heroDamageLv < 30)
                    {

                        SoundEffectManager.effectUpgradeAction();
                        Debug.Log($"히어로데미지업");
                        DataManager.GetInstance().userInfo.heroDamageLv++;
                    }
                    else if (DataManager.GetInstance().userInfo.heroDamageLv == 30)
                    {
                        Debug.Log("업그레이드 완료");
                    }
                    DisplayHeroInfo();
                    DataManager.GetInstance().SaveUserInfo();
                }
                else
                {
                    Debug.Log("돈없쪙");
                }
            }
            else
            {
                Debug.Log("HeroDamageLV 30-------MAX");
            }
        }));

        this.btnUpgradeHeroAspdLV.onClick.Add(new EventDelegate(() =>
        {
            var heroAttackSpeedLv = DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().userInfo.heroAtkSpeedLv + 1].level;
            if (heroAttackSpeedLv <= 30)
            {
                if (DataManager.GetInstance().userInfo.cash >= DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().userInfo.heroAtkSpeedLv + 1].gold)
                {
                    DataManager.GetInstance().userInfo.cash -= (ulong)DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().userInfo.heroAtkSpeedLv + 1].gold;
                    onChangeMoney();
                    if (DataManager.GetInstance().userInfo.heroAtkSpeedLv < 30)
                    {

                        SoundEffectManager.effectUpgradeAction();
                        Debug.Log($"히어로공속업");
                        DataManager.GetInstance().userInfo.heroAtkSpeedLv++;
                    }
                    else if (DataManager.GetInstance().userInfo.heroAtkSpeedLv == 30)
                    {
                        Debug.Log("업그레이드 완료");
                    }
                    DisplayHeroInfo();
                    DataManager.GetInstance().SaveUserInfo();
                }
                else
                {
                    Debug.Log("돈없쪙");
                }
            }
            else
            {
                Debug.Log("HeroAttackSpeedLV 30-------MAX");
            }
        }));
    }
    private int GetPresentCommander_id()
    {
        var selectedData = (from info in DataManager.GetInstance().dicCommanderinfo
                            where info.Value.isSelected == true
                            select info.Value).FirstOrDefault();
        return selectedData.commander_id;
    }

    private void SetCommander(int commander_id)
    {
        if (commanderGo != null)
        {
            Destroy(commanderGo);
        }

        var data = DataManager.GetInstance().dicCommanderData[commander_id];
        var prefab = (GameObject)(from obj in App.instance.resource
                                  where obj.name == data.prefab_name
                                  select obj).FirstOrDefault();
        commanderGo = Instantiate(prefab);
        commanderGo.transform.SetParent(commanderIconPos.transform, false);

        SetLayersRecursively(commanderGo.transform, "UI");

        commanderGo.transform.rotation = Quaternion.Euler(0, 180, 0);

        commanderGo.transform.localScale = commanderScale;
        var anim = commanderGo.GetComponentInChildren<Animator>();
        anim.Play($"MyInfo", -1, 0);
    }

    public void SetLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            SetLayersRecursively(child, name);
        }
    }
    public void DisplayInfos()
    {
        presentComm_id = GetPresentCommander_id();
        SetCommander(presentComm_id);

        DisplayCommanderInfo();
        DisplayHeroInfo();
    }
    public void DisplayCommanderInfo()
    {
        lbCommanderDamage.text = DataManager.GetInstance().dicCommanderinfo[presentComm_id].damageTotal.ToString();
        lbCommanderAspd.text = DataManager.GetInstance().dicCommanderinfo[presentComm_id].atkSpeedTotal.ToString();
        if (DataManager.GetInstance().dicCommanderinfo[presentComm_id].damageLv + 1 <= 30)
        {
            lbCommanderDmGold.text = DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().dicCommanderinfo[presentComm_id].damageLv + 1].gold.ToString() + " G";
        }
        else if (DataManager.GetInstance().dicCommanderinfo[presentComm_id].damageLv == 30)
        {
            lbCommanderDmGold.text = "업그레이드 완료";
        }
        if (DataManager.GetInstance().dicCommanderinfo[presentComm_id].atkSpeedLV + 1 <= 30)
        {
            lbCommanderAsGold.text = DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().dicCommanderinfo[presentComm_id].atkSpeedLV + 1].gold.ToString() + " G";
        }
        else if (DataManager.GetInstance().dicCommanderinfo[presentComm_id].atkSpeedLV == 30)
        {
            lbCommanderAsGold.text = "업그레이드 완료";
        }
        lbCommanderDamageLV.text = DataManager.GetInstance().dicCommanderinfo[presentComm_id].damageLv.ToString();
        lbCommanderAspdLV.text = DataManager.GetInstance().dicCommanderinfo[presentComm_id].atkSpeedLV.ToString();
    }

    public void DisplayHeroInfo()
    {
        lbHeroDamageLV.text = DataManager.GetInstance().userInfo.heroDamageLv.ToString();
        lbHeroAspdLV.text = DataManager.GetInstance().userInfo.heroAtkSpeedLv.ToString();
        if (DataManager.GetInstance().userInfo.heroDamageLv + 1 <= 30)
        {
            lbHeroDmGold.text = DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().userInfo.heroDamageLv + 1].gold.ToString() + " G";
        }
        else if (DataManager.GetInstance().userInfo.heroDamageLv == 30)
        {
            lbHeroDmGold.text = "업그레이드 완료";
        }
        if (DataManager.GetInstance().userInfo.heroAtkSpeedLv + 1 <= 30)
        {
            lbHeroAsGold.text = DataManager.GetInstance().dicUpgradeData[DataManager.GetInstance().userInfo.heroAtkSpeedLv + 1].gold.ToString() + " G";
        }
        else if (DataManager.GetInstance().userInfo.heroAtkSpeedLv == 30)
        {
            lbHeroAsGold.text = "업그레이드 완료";
        }
    }
    public void Open()
    {
        DisplayInfos();

    }
}
