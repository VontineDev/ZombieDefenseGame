using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTime : MonoBehaviour
{
    public UISprite dim;
    public UILabel count;

    public void DimTime()
    {

    }
    public void CountTime()
    {

    }
    IEnumerator dimTimeCor()
    {
        while (true)
        {
            dim.fillAmount -= 0.1f;
            if(dim.fillAmount == 0)
            {
                dim.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
    }
    

}
