using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_Credit : MonoBehaviour
{
    public UIButton btnClose;

    internal void Init()
    {

        this.btnClose.onClick.Add(new EventDelegate(() =>
        {
            this.gameObject.SetActive(false);
        }));
    }

    internal void Open()
    {
        this.gameObject.SetActive(true);

    }
}
