using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UIPanel_Money : MonoBehaviour
{
    private UserInfo userInfo;
    public UILabel lbCashAmount;
    //public UILabel lbDiamondAmount;
    public Action<int> OnMoneyChange;
    public void Init()
    {
        this.userInfo = DataManager.GetInstance().userInfo;
        OnMoneyChange = (cashToAdd) =>
        {
            SetMoney(cashToAdd);
            DisplayMoney();
        };
    }
    public void DisplayMoney()
    {
        this.lbCashAmount.text = NormalizeCash();
    }
    //단위 표준화
    private string NormalizeCash()
    {
        string strCash = "";

        if (this.userInfo.cash / 1000000000000 > 0)
        {
            var cash = this.userInfo.cash / 1000000000;
            strCash = GetThousandCommaText(cash) + "G";
        }
        else if (this.userInfo.cash / 1000000000 > 0)
        {
            ulong cash = this.userInfo.cash / 1000000;
            strCash = GetThousandCommaText(cash) + "M";
        }
        else if (this.userInfo.cash / 1000000 > 0)
        {
            var cash = this.userInfo.cash / 1000;
            strCash = GetThousandCommaText(cash) + "K";
        }
        else
        {
            var cash = this.userInfo.cash;
            strCash = GetThousandCommaText(cash);
            if (strCash == "")
            {
                strCash = "0";
            }
        }

        return strCash;
    }

    public string GetThousandCommaText(ulong data)
    {
        return string.Format("{0:#,###}", data);
    }

    public void SetMoney(int cashToAdd = 0)
    {
        this.userInfo.cash += (ulong)cashToAdd;
    }

}
