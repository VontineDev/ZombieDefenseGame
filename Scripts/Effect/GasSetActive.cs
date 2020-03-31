using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSetActive : MonoBehaviour
{
    public void SetActive()
    {
        var findGo = TransformDeepChildExtension.FindDeepChild(this.transform, "SA_Wep_Grenade").gameObject;
        findGo.SetActive(true);
    }
}
