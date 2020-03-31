using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class UI_btnThumb : MonoBehaviour
{
    public int savedHeroType;
    public UIButton thumb;

    public GameObject prefab;
    public GameObject go;
    public Coroutine coBehavior;
    public Action OnDragStartCallback;
    public Action OnDragEndCallback;
    public Action OnIdle;
    public Action<TestHero> OnMergeHero;
    public Action<int, Vector3> OnPutDownHero;

    public enum eBtnThumb { IDLE, DRAG, RELEASE }
    public void Init()
    {
        this.thumb = this.gameObject.GetComponent<UIButton>();
    }

    #region CHANGE_STATE
    public void ChangeState(eBtnThumb state)
    {
        switch (state)
        {
            case eBtnThumb.IDLE:
                {
                    Idle();
                }
                break;
            case eBtnThumb.DRAG:
                {
                    Drag();
                }
                break;
            case eBtnThumb.RELEASE:
                {
                    Release();
                }
                break;
            default:
                break;
        }

    }
    #endregion

    #region DRAG
    private void Drag()
    {
        MakeGameObject();
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        StartCoroutine(DragImpl());
    }
    IEnumerator DragImpl()
    {
        while (true)
        {
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 1f);
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Plane");
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                //Debug.Log(hit.point);
                if (go != null)
                    go.transform.position = hit.point;
            }
        }
    }


    #endregion

    #region RELEASE
    private void Release()
    {
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        StartCoroutine(ReleaseImpl());

    }
    IEnumerator ReleaseImpl()
    {
        if (go != null)
        {
            Destroy(go);
            go = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 1f);

        RaycastHit hit2;
        int layerMask = 1 << LayerMask.NameToLayer("Hero");

        if (Physics.Raycast(ray, out hit2, 100, layerMask)) //레이캐스트가 히어로 레이어 맞춤
        {
            var otherHero = hit2.transform.root.GetComponent<TestHero>();
            if (savedHeroType == otherHero.data.type)
            {
                OnMergeHero(otherHero);
            }
        }
        else if (Physics.Raycast(ray, out hit2, 100)) //레이캐스트를 쏨
        {
            if (hit2.transform.gameObject.tag == "Foothold") //Foothold 발판을 맞춤
            {
                Debug.Log("========================OnCollocateHero=================");
                if (savedHeroType != 0)
                {
                    OnPutDownHero(savedHeroType, hit2.transform.position);
                }
            }
        }
        else
        {
            //아이들로 감
            OnIdle();
        }
        yield return null;
    }
    #endregion

    #region IDLE
    private void Idle()
    {
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        StartCoroutine(IdleImpl());
    }
    IEnumerator IdleImpl()
    {
        Debug.Log("IdleImpl");
        ResetThumb();
        yield return null;
    }
    #endregion

    private void ResetThumb()
    {
        savedHeroType = 0;
        if (go != null)
        {
            Destroy(go);
            go = null;
        }
        SetThumb(savedHeroType);
    }
    public void OnDragStart()
    {
        OnDragStartCallback();
    }

    private void MakeGameObject()
    {
        if (savedHeroType != 0)
        {
            var data = DataManager.GetInstance().dicHeroData[savedHeroType];
            if (data != null)
            {
                var prefab = (GameObject)(from obj in App.instance.resource
                                          where obj.name == data.prefab_name
                                          select obj).FirstOrDefault();
                this.prefab = prefab;
            }
        }
        switch (savedHeroType)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                {
                    go = Instantiate(prefab);
                    go.layer = LayerMask.NameToLayer("Default");
                }
                break;
            default:
                {
                    SetThumb(0);
                }
                break;
        }
    }


    public void OnDragEnd()
    {
        OnDragEndCallback();
    }


    public void CreateNewHero()
    {

        if (savedHeroType != 0)
        {
            while (true)
            {

                //30% 확률로 2성을 뽑을 수 있다.
                var rand_heroLv = UnityEngine.Random.Range(1, 11);
                int rand = 0;
                if (rand_heroLv < 8)
                {
                    rand = UnityEngine.Random.Range(1, 6);
                }
                else
                {
                    rand = UnityEngine.Random.Range(6, 11);
                }

                //rand = UnityEngine.Random.Range(1, 6);

                if (savedHeroType != rand)
                {
                    SetThumb(rand);
                    this.savedHeroType = rand;

                    break;
                }
            }
        }
        else
        {
            var rand_heroLv = UnityEngine.Random.Range(1, 11);
            int rand = 0;
            if (rand_heroLv < 8)
            {
                rand = UnityEngine.Random.Range(1, 6);
            }
            else
            {
                rand = UnityEngine.Random.Range(6, 11);
            }

            //var rand = UnityEngine.Random.Range(1, 6);

            SetThumb(rand);
            this.savedHeroType = rand;
        }
    }

    private void SetThumb(int number)
    {
        switch (number)
        {
            case 0:
                {
                    this.thumb.normalSprite = "None";
                    this.savedHeroType = 0;
                }
                break;
            case 1:
                {
                    this.thumb.normalSprite = "Hero02_1";
                }
                break;
            case 2:
                {
                    this.thumb.normalSprite = "Hero03_1";
                }
                break;
            case 3:
                {
                    this.thumb.normalSprite = "Hero04_1";
                }
                break;
            case 4:
                {
                    this.thumb.normalSprite = "Hero05_1";
                }
                break;
            case 5:
                {
                    this.thumb.normalSprite = "Hero06_1";
                }
                break;
            case 6:
                {
                    this.thumb.normalSprite = "Hero02_2";
                }
                break;
            case 7:
                {
                    this.thumb.normalSprite = "Hero03_2";
                }
                break;
            case 8:
                {
                    this.thumb.normalSprite = "Hero04_2";
                }
                break;
            case 9:
                {
                    this.thumb.normalSprite = "Hero05_2";
                }
                break;
            case 10:
                {
                    this.thumb.normalSprite = "Hero06_2";
                }
                break;
            default:
                {
                    this.thumb.normalSprite = "None";
                    this.savedHeroType = 0;
                }
                break;
        }
    }
}


