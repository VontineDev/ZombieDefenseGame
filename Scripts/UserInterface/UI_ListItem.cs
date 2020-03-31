using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UI_ListItem : MonoBehaviour
{
    public UIButton button;
    public UISprite selected;
    public UIButton icon;
    public ItemInfo itemInfo;
    public Action<int> OnSelectItem;
    private void Start()
    {
        CheckSelectedItem(false);

        this.button.onClick.Add(new EventDelegate(() =>
        {
            OnSelectItem(this.itemInfo.id);   
        }));

    }
    public void SetItem(string icon_name, ItemInfo itemInfo)
    {
        this.icon.normalSprite = icon_name;
        this.itemInfo = itemInfo;
    }
    public void CheckSelectedItem(bool isActivated)
    {
        this.selected.gameObject.SetActive(isActivated);
    }

}
