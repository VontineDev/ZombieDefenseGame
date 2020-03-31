using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_HeroLevel : MonoBehaviour
{
    public GameObject uI_HeroLv1;
    public GameObject uI_HeroLv2;
    public GameObject uI_HeroLv3;

    public void SetLevel(int lv)
    {
        switch (lv)
        {
            case 0:
                {
                    HideAll();
                }
                break;
            case 1:
                {
                    DisplayLv1();
                }
                break;
            case 2:
                {
                    DisplayLv2();
                }
                break;
            case 3:
                {
                    DisplayLv3();
                }
                break;
            default:
                break;
        }
    }
    private void HideAll()
    {
        this.uI_HeroLv1.SetActive(false);
        this.uI_HeroLv2.SetActive(false);
        this.uI_HeroLv3.SetActive(false);
    }
    private void DisplayLv1()
    {
        this.uI_HeroLv1.SetActive(true);
        this.uI_HeroLv2.SetActive(false);
        this.uI_HeroLv3.SetActive(false);
    }
    private void DisplayLv2()
    {
        this.uI_HeroLv1.SetActive(false);
        this.uI_HeroLv2.SetActive(true);
        this.uI_HeroLv3.SetActive(false);
    }
    private void DisplayLv3()
    {
        this.uI_HeroLv1.SetActive(false);
        this.uI_HeroLv2.SetActive(false);
        this.uI_HeroLv3.SetActive(true);
    }
}