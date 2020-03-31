using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNGUIPopup : MonoBehaviour
{
    public UIRoot uiRoot;
    public UIPopup_InGameSetting uiPopup_InGameSetting;
    // Start is called before the first frame update
    void Start()
    {
        if (this.uiPopup_InGameSetting != null)
        {
            this.uiPopup_InGameSetting.Open();
        }
        else
        {
            //var prefab = (GameObject)(from obj in App.instance.resource
            //                          where obj.name == "UIPopup_InGameSetting"
            //                          select obj).FirstOrDefault();
            var prefab = Resources.Load<GameObject>("Prefab/UI/UIPopup_InGameSetting");
            this.uiPopup_InGameSetting = Instantiate<GameObject>(prefab).GetComponent<UIPopup_InGameSetting>();

            this.uiPopup_InGameSetting.transform.SetParent(this.uiRoot.transform);
            this.uiPopup_InGameSetting.transform.localScale = Vector3.one;
            this.uiPopup_InGameSetting.transform.localPosition = Vector3.zero;

            this.uiPopup_InGameSetting.Init();
        }
    }
}
