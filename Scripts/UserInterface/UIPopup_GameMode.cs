using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup_GameMode : MonoBehaviour
{
    enum eGameMode { DefenseMode, DungeonMode, InfiniteMode }
    public UIButton btnClose;
    public UIButton[] btnMode;
    // Start is called before the first frame update
    void Start()
    {
        this.btnClose.onClick.Add(new EventDelegate(() =>
        {
            this.gameObject.SetActive(false);
        }));
        for (int i = 0; i < btnMode.Length; i++)
        {
            var capt = i;
            this.btnMode[capt].onClick.Add(new EventDelegate(() =>
            {
                SceneLoader.GetInstance().LoadSingleScene(((eGameMode)capt).ToString());
            }));
        }
    }
}
