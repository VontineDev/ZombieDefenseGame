using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FenceHp : MonoBehaviour
{
    public UISprite fenceHp;

    public void Init()
    {
        fenceHp.fillAmount = 1;
    }

    public void ChangeFillAmount(float percentage)
    {
        fenceHp.fillAmount = percentage;
    }
}

