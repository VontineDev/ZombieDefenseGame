using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPopup : MonoBehaviour
{
    public UIButton btn;
    // Start is called before the first frame update
    void Start()
    {
        this.btn.onClick.Add(new EventDelegate(() =>
       {
           Debug.Log("테스트버튼눌림");
       }));  
    }
}
