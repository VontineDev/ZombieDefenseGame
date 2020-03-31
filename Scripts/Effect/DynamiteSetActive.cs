using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteSetActive : MonoBehaviour
{
    public void SetAttive()
    {
        var findGo = TransformDeepChildExtension.FindDeepChild(this.transform, "SA_Wep_DynamiteBundle").gameObject;
        findGo.SetActive(true);
    }
}
