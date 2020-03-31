using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItle : MonoBehaviour
{
    public UILabel title;
    //public Color deColor;
    public UISprite dimSprite;
    public UIButton btn;
    public System.Action OnTitleClicked;
    private void Start()
    {
        //var color = title.color;
        //color.a = 0;
        //deColor = color;
        //StartCoroutine(DimTitle1());
        this.Init();
    }

    private void Init() {
        DOTween.ToAlpha(() => this.dimSprite.color, x => this.dimSprite.color = x, 0, 1).SetEase(Ease.OutQuad).onComplete = () => {
            Debug.Log("fade out comple");

            Debug.Log("wait for click");
            this.btn.onClick.Add(new EventDelegate(() => {
                DOTween.ToAlpha(() => this.dimSprite.color, x => this.dimSprite.color = x, 1, 1).SetEase(Ease.InQuad).onComplete = () =>
                {
                    Debug.Log("fade in comple");

                    this.OnTitleClicked();
                };
            }));
        };
    }

    //IEnumerator DimTitle1()
    //{
    //    var color = title.color;
    //    while (true)
    //    {
    //        yield return null;
    //        color.a += 0.025f;
    //        title.color = color;
    //        if (title.color.a >= 1)
    //        {
    //            StartCoroutine(DimTitle2());
    //            break;
    //        }
    //    }
    //}
    //IEnumerator DimTitle2()
    //{
    //    var color = title.color;
    //    while (true)
    //    {
    //        yield return null;
    //        color.a -= 0.025f;
    //        title.color = color;
    //        if (title.color.a <= 0)
    //        {
    //            title.color = deColor;
    //            App.instance.OnCompleteScene();
    //            break;
    //        }
    //    }
    //}
}
