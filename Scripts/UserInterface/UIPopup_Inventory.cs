using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class UIPopup_Inventory : MonoBehaviour
{
    public UIButton btnClose;
    public UI_ListItem[] arrUI_ListItems;

    public UIButton btnWearItem;
    public UIButton btnBuyItem;

    //캐릭터가 착용하고있는 아이템의 아이콘들
    public UISprite iconRing;
    public UISprite iconNecklace;

    //장비 정보창의 아이콘, 라벨
    public UISprite iconWearingItem;
    public UILabel lbWearingDmg;
    public UILabel lbWearingAspd;

    public UISprite iconSelectedItem;
    public UILabel lbSelectedDmg;
    public UILabel lbSelectedAspd;

    public int selectedItem_id;
    public int wearingItem_id;

    public GameObject commanderIconPos;
    public GameObject commanderGo;
    public Vector3 commaderPosition = new Vector3(0, 0, 0);
    public Vector3 commanderScale = new Vector3(150, 150, 150);
    //Commander
    private int presentComm_id;
    public void Start()
    {
        btnClose.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            DataManager.GetInstance().SaveItemInfo();
            this.gameObject.SetActive(false);
        }));
        DisplayCharacterStatus();

        this.btnBuyItem.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            if (true)//아이템 가격보다 많은 돈을 들고 있다면 사도록
            {
                Debug.Log("구매합니다");
                if (this.selectedItem_id != 0)
                {
                    DataManager.GetInstance().BuyItem(this.selectedItem_id);
                    this.btnBuyItem.gameObject.SetActive(false);
                    this.btnWearItem.gameObject.SetActive(true);
                }
            }
        }));

        this.btnWearItem.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            if (this.selectedItem_id != 0 && this.wearingItem_id != 0)
            {
                DataManager.GetInstance().UnEquipItem(this.wearingItem_id);
                DataManager.GetInstance().EquipItem(this.selectedItem_id);
                DisplayCharacterStatus();
                DisplayEquipmentStatus();
            }

        }));

        for (int i = 0; i < arrUI_ListItems.Length; i++)
        {
            int capt = i;
            arrUI_ListItems[capt].SetItem(DataManager.GetInstance().dicItemData[capt + 1].icon_name, DataManager.GetInstance().dicItemInfo[capt + 1]);

            arrUI_ListItems[capt].OnSelectItem = (id) =>
            {
                UnCheckAllItem();
                arrUI_ListItems[capt].CheckSelectedItem(true);

                if (DataManager.GetInstance().dicItemInfo[id].isPossessed == true)
                {
                    this.btnBuyItem.gameObject.SetActive(false);
                    this.btnWearItem.gameObject.SetActive(true);
                }
                else
                {
                    this.btnBuyItem.gameObject.SetActive(true);
                    this.btnWearItem.gameObject.SetActive(false);
                }
                selectedItem_id = id;
                DisplayCharacterStatus();
                DisplayEquipmentStatus();
            };
        }
    }

    internal void Init()
    {
        presentComm_id = GetPresentCommander_id();
        SetCommander(presentComm_id);
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

    public void DisplayCharacterStatus()
    {
        var itemData = DataManager.GetInstance().dicItemData;
        var itemInfo = DataManager.GetInstance().dicItemInfo;
        var item_id = (from item in itemInfo
                       where item.Value.isEquipped == true
                       select item.Value.id).ToArray();

        foreach (var id in item_id)
        {
            Debug.Log($"item_id:{id}");

            if (itemData[id].type == 1)
            {
                SetIcon(iconRing, itemData[id].icon_name);
            }
            else if (itemData[id].type == 2)
            {
                SetIcon(iconNecklace, itemData[id].icon_name);
            }
        }
    }
    public void SetIcon(UISprite sprite, string icon_name)
    {
        sprite.spriteName = icon_name;
    }
    public void DisplayEquipmentStatus()
    {
        var itemData = DataManager.GetInstance().dicItemData;
        var itemInfo = DataManager.GetInstance().dicItemInfo;

        if (selectedItem_id != 0)
        {
            SoundEffectManager.effectSoundAction();
            var item_id = (from item in itemData
                           where item.Value.id == selectedItem_id
                           select item.Value.id).First();

            SetIcon(iconSelectedItem, itemData[item_id].icon_name);

            SetLabel(lbSelectedDmg, itemData[item_id].damage, lbSelectedAspd, itemData[item_id].atkSpeed);
            var type = itemData[item_id].type;

            var arrIds = (from item in itemInfo
                          where item.Value.isEquipped == true
                          select item.Value.id).ToArray();

            Debug.Log($"Selected:{selectedItem_id}");
            //Debug.Log($"wearing arrIds:{arrIds[0]}, {arrIds[1]}");

            var wearingItem_id = (from id in arrIds
                                  where itemData[id].type == type
                                  select id).First();
            Debug.Log($"wearingItem_id: {wearingItem_id}");
            this.wearingItem_id = wearingItem_id;
            SetIcon(iconWearingItem, itemData[wearingItem_id].icon_name);
            SetLabel(lbWearingDmg, itemData[wearingItem_id].damage, lbWearingAspd, itemData[wearingItem_id].atkSpeed);
        }
    }
    public void SetLabel(UILabel lbDamage, float damage, UILabel lbAspd, float aspd)
    {
        lbDamage.text = damage.ToString();
        lbAspd.text = aspd.ToString();
    }
    public void UnCheckAllItem()
    {
        foreach (var item in arrUI_ListItems)
        {
            item.CheckSelectedItem(false);
        }
    }
}
