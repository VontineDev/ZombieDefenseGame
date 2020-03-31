using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UI_UserInteraction : MonoBehaviour
{

    public GameObject nguiRoot;
    public UI_btnThumb uI_BtnThumb; //썸네일 버튼쪽 스크립트
    public UIButton btnNewRandom; //새로운 랜덤영웅 생성 버튼   

    public GameObject skillDimBg;
    public UIButton btnSkill; //스킬버튼
    public UISprite skillDim;
    public UILabel skillCount;

    public UIButton btnRollHero; //기존 영웅 변경 버튼
    public TestHero SelectedHero; //터치로 선택된 영웅
    public TestHero HeroToRoll; //터치로 선택된 영웅

    public Action OnDragHero; //영웅 드래그시 호출
    public Action OnPressNothing; //아무것도 눌리지 않았을때 호출
    public Action OnRelease; //터치를 뗄 때 호출
    public Action OnPress; //터치 누를때 호출

    public Action useSkillAction;
    public Action<TestHero> OnChangeHero; //영웅 교체시 호출
    public Action OnCreateNewHero; //새 영웅 생성시 호출
    public Action<TestHero> OnMergeHeroWithUiThumbnail; //새 영웅과 합칠 때 호출
    public Action<TestHero, TestHero> OnMergeHeroWithGameObject; //기존 영웅끼리 합칠 때 호출
    
    public Action SkillDimAction;
   
    public UILabel lbHeroDm;
    public UILabel lbHeroAs;


    public eTouchState currentState; //현재 상태
    public enum eTouchState { PRESS_NOTHING, PRESS, DRAG, RELEASE }

    public Coroutine coBehavior; //상태 코루틴 (1가지 상태만 가지도록함)
    public void Start()
    {
        skillDim.gameObject.SetActive(false);
        skillCount.gameObject.SetActive(false);
    }
    public void Init()
    {
        uI_BtnThumb.Init();

        uI_BtnThumb.OnMergeHero = (otherHero) =>
        {
            OnMergeHeroWithUiThumbnail(otherHero);
            uI_BtnThumb.ChangeState(UI_btnThumb.eBtnThumb.IDLE);
        };
        uI_BtnThumb.OnIdle = () =>
        {
            uI_BtnThumb.ChangeState(UI_btnThumb.eBtnThumb.IDLE);
        };
        uI_BtnThumb.OnDragStartCallback = () =>
        {
            uI_BtnThumb.ChangeState(UI_btnThumb.eBtnThumb.DRAG);
        };
        uI_BtnThumb.OnDragEndCallback = () =>
         {
             uI_BtnThumb.ChangeState(UI_btnThumb.eBtnThumb.RELEASE);
         };
        this.btnNewRandom.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            OnCreateNewHero();
        }));
        DisplayState();

        this.btnRollHero.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            OnChangeHero(HeroToRoll);
        }));
        this.btnSkill.onClick.Add(new EventDelegate(() =>
        {
            SoundEffectManager.effectSoundAction();
            SkillDimSprite(); // UI화면에 스킬 사용한다고 알려줌
            SkillDimAction = () =>
            {
                btnSkill.GetComponent<UIButton>().isEnabled = false;
                SkillCoolTimeDim();
                SkillCoolTimeLabel();
                useSkillAction();
                //액션으로 디펜스모드에 전달(디펜스모드에서 데미지를 관리)
                //var jet = Resources.Load<GameObject>("Prefab/Effect/Jet");
                //Instantiate(jet);
            };


        }));

    }

    public void ChangeState(eTouchState state)
    {
        switch (state)
        {
            case eTouchState.PRESS:
                {
                    Press();
                }
                break;
            case eTouchState.PRESS_NOTHING:
                {
                    PressNothing();
                }
                break;
            case eTouchState.DRAG:
                {
                    Drag();
                }
                break;
            case eTouchState.RELEASE:
                {
                    Release();
                }
                break;          
            default:
                {
                    Debug.Log($"UI_UserInteraction.ChangeState: default");
                }
                break;

        }
    }

    //현재상태 출력
    private void DisplayState()
    {
        Debug.Log($"현재 상태{this.currentState.ToString()}");
    }

    //현재 터치의 상태 확인
    public void Check()
    {
        StartCoroutine(CheckImpl());
    }
    IEnumerator CheckImpl()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnPress();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnRelease();
            }
            yield return null;
        }
    }

    #region PRESS
    private void Press()
    {
        this.currentState = eTouchState.PRESS;
        DisplayState();
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(PressImpl());
    }
    IEnumerator PressImpl()
    {
        currentState = eTouchState.PRESS;
        while (true)
        {
            yield return null;
            //터치하면 레이를 쏜다
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green, 1f);
            var layerMask = 1 << LayerMask.NameToLayer("Hero");
            if (Physics.Raycast(ray, out hit, 100, layerMask))  //영웅 레이어에 쐈을때 감지되는가?
            {
                Debug.Log($"PressImpl: Hit:{hit.transform.gameObject.GetComponent<TestHero>()}");
                SelectedHero = hit.transform.gameObject.GetComponent<TestHero>();
                OnDragHero();
                break;
            }
            else
            {
                OnPressNothing();
            }
        }
    }
    #endregion

    #region PRESSNOTHING
    private void PressNothing()
    {
        this.currentState = eTouchState.PRESS_NOTHING;
        DisplayState();
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(PressNothingImpl());
    }
    IEnumerator PressNothingImpl()
    {
        currentState = eTouchState.PRESS_NOTHING;
        while (true)
        {
            //선택된 히어로가 있으면 돌아간다
            if (SelectedHero != null)
            {
                SelectedHero.transform.position = SelectedHero.defaultPos;
                this.SelectedHero = null;
            }
            yield return null;
            break;
        }

    }
    #endregion    

    #region DRAG
    private void Drag()
    {
        this.currentState = eTouchState.DRAG;
        DisplayState();
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(DragImpl());

    }
    IEnumerator DragImpl()
    {
        currentState = eTouchState.DRAG;
        while (true) //화면을 누르고있는상태
        {
            yield return null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//Plane에 쏘는레이
            RaycastHit hit;
            var layerMask2 = 1 << LayerMask.NameToLayer("Plane");
            Debug.DrawRay(ray.origin, ray.direction * 1000, Color.cyan, 1f);
            if (Physics.Raycast(ray, out hit, 100, layerMask2))//Plane과 충돌 확인
            {
                if (SelectedHero != null)
                {
                    var point = hit.point;
                    point.y += 1.5f;
                    SelectedHero.transform.position = point; //내 멤버변수의 히어로의 위치를 터치가 바닥과 닿은곳에서 y는 1.5만큼 떨어진 곳으로 이동한다.
                }
            }
            else
            {
                //Plane과 터치하지 않았으므로 되돌아간다
                SelectedHero.transform.position = SelectedHero.defaultPos;
                SelectedHero = null;
                break;
            }

        }

    }
    #endregion

    #region RELEASE
    private void Release()
    {
        this.currentState = eTouchState.RELEASE;
        DisplayState();
        if (coBehavior != null)
        {
            StopCoroutine(coBehavior);
        }
        coBehavior = StartCoroutine(ReleaseImpl());
    }
    IEnumerator ReleaseImpl()
    {
        currentState = eTouchState.RELEASE;
        var layerMask = 1 << LayerMask.NameToLayer("Hero");

        if (SelectedHero != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))//마우스포지션에서 레이를 쏜다.
            {
                var otherHero = hit.transform.gameObject.GetComponent<TestHero>();

                if (otherHero == SelectedHero)
                {
                    SelectedHero.transform.position = SelectedHero.defaultPos;
                    DisPlayHeroRandButton();
                }
                //두개의 타입을 체크한다
                else
                {
                    if (SelectedHero.data.type == otherHero.data.type && SelectedHero.data.type < 11)
                    {
                        //같은 타입이므로 합칠 수 있다
                        OnMergeHeroWithGameObject(SelectedHero, otherHero);
                        HideHeroRandButton();
                    }
                }
            }
            else
            {
                SelectedHero.transform.position = SelectedHero.defaultPos;
                DisPlayHeroRandButton();
            }
        }
        yield return null;
        OnPressNothing();
    }
    #endregion      

    //btnRollHero 숨기기
    public void HideHeroRandButton()   
    {       
        btnRollHero.gameObject.SetActive(false);
    }

    //btnRollHero 보이기
    private void DisPlayHeroRandButton()
    {      
        HeroToRoll = SelectedHero;
        if (SelectedHero)
        {
            lbHeroDm.text = "선택한 영웅 공격력 : " + SelectedHero.data.damage.ToString();
            lbHeroAs.text = "선택한 영웅 공격속도 : " + SelectedHero.data.atkSpeed.ToString();
        }
        btnRollHero.gameObject.SetActive(true);

        if (SelectedHero.data.type > 5)
        {
            btnRollHero.gameObject.SetActive(false);
        }
       
        if (HeroToRoll != null)
        {
            var pos = HeroToRoll.defaultPos + Vector3.up * 3 + Vector3.forward * 2f;
            Vector3 v = Camera.main.WorldToScreenPoint(pos);       
            v.x = (v.x / Screen.width) * 1080;
            v.y = (v.y / Screen.height) * 1920;        
            btnRollHero.transform.localPosition = v - new Vector3(1080 / 2, 1920 / 2, 0) - 50 * Vector3.up;          
        }
    }

    #region Skill
    public void SkillCoolTimeDim()
    {
        skillDim.gameObject.SetActive(true);
        skillDim.fillAmount = 1f;
        StartCoroutine(SkillDimCor());
    }
    IEnumerator SkillDimCor()
    {
        float a = 0;
        while (true)
        {
            a += Time.deltaTime;
            skillDim.fillAmount = (30 - a) / 30;

            if (skillDim.fillAmount == 0)
            {
                btnSkill.GetComponent<UIButton>().isEnabled = true;
                skillDim.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }

    }
    public void SkillCoolTimeLabel()
    {
        skillCount.gameObject.SetActive(true);
        skillCount.text = (10).ToString();
        StartCoroutine(SkillCountCor());
    }
    IEnumerator SkillCountCor()
    {
        for (int i = 1; i <= 30; i++)
        {
            skillCount.text = (30 - i).ToString();
            if (i == 30)
            {
                skillCount.gameObject.SetActive(false);
                break;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void SkillDimSprite()
    {
        StartCoroutine(SkillDimCoroutine());
    }
    IEnumerator SkillDimCoroutine()
    {
        Time.timeScale = 0;
        skillDimBg.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
        skillDimBg.SetActive(false);
        SkillDimAction();

    }
    #endregion
}
