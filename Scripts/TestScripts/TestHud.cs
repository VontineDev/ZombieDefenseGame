using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHud : MonoBehaviour
{
    public UIButton hudBtn;
    public UILabel hudLabel;

    private void Start()
    {
        hudBtn.onClick.Add(new EventDelegate(()=> {
            hudLabel.text = "12";
        }));
    }
}
