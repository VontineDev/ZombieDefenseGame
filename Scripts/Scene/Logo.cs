using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Logo : MonoBehaviour
{
    public UILabel logo;
    public Color deColor;
    public UISprite dimSprite;
    public System.Action OnLogoShown;
    public UISprite googlePlayCi;
    public UISprite logoCi;

    private void Start()
    {
        //var color = logo.color;
        //color.a = 0;
        //deColor = color;      
        //StartCoroutine(DimLogo1());
        this.logoCi.gameObject.SetActive(true);
        this.googlePlayCi.gameObject.SetActive(false);
        this.Init();
    }
    public void Init() 
    {
        DOTween.ToAlpha(() => this.dimSprite.color, x => this.dimSprite.color = x, 0, 1f).SetEase(Ease.OutQuad).onComplete = () => {
            Debug.Log("fade out comple");

            StartCoroutine(this.WaitForSec(1.5f, () => {
                DOTween.ToAlpha(() => this.dimSprite.color, x => this.dimSprite.color = x, 1, 1f).SetEase(Ease.InQuad).onComplete = () => {
                    Debug.Log("fade in comple");

                    this.logoCi.gameObject.SetActive(false);
                    this.googlePlayCi.gameObject.SetActive(true);
                    DOTween.ToAlpha(() => this.dimSprite.color, x => this.dimSprite.color = x, 0, 1f).SetEase(Ease.OutQuad).onComplete = () => {
                        Debug.Log("fade out comple");

                        StartCoroutine(this.WaitForSec(1.5f, () => {
                            DOTween.ToAlpha(() => this.dimSprite.color, x => this.dimSprite.color = x, 1, 1f).SetEase(Ease.InQuad).onComplete = () => {
                                Debug.Log("fade in comple");

                                this.OnLogoShown();
                            };
                        }));
                    };
                };
            }));
        };
    }

    private IEnumerator WaitForSec(float t, System.Action onComplete) {
        yield return new WaitForSeconds(t);
        onComplete();
    }
    
    //IEnumerator DimLogo1()
    //{
    //    var color = logo.color;
    //    while (true)
    //    {
    //        yield return null;
    //        color.a += 0.025f;
    //        logo.color = color;
    //        if (logo.color.a >= 1)
    //        {
    //            StartCoroutine(DimLogo2());
    //            break;
    //        }
    //    }
    //}
    //IEnumerator DimLogo2()
    //{
    //    var color = logo.color;
    //    while (true)
    //    {
    //        yield return null;
    //        color.a -= 0.025f;
    //        logo.color = color;
    //        if (logo.color.a <= 0)
    //        {
    //            logo.color = deColor;
    //            App.instance.OnCompleteScene();
    //            break;
    //        }
    //    }
    //}
}
