using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_HeroStatus : MonoBehaviour
{
    public UIButton btnClose;
    public UIPopup_MyInfo ui_MyInfo;
    public UILabel[] lbSkills;
    public GameObject heroPos;
    public GameObject heroGo;
    private Vector3 heroScale = new Vector3(150, 150, 150);
    public UIButton btnPrev;
    public UIButton btnNext;
    private int hero_id = 1;
    private void Start()
    {
        btnClose.onClick.Add(new EventDelegate(() => {
            SoundEffectManager.effectSoundAction();
            this.gameObject.SetActive(false);
            ui_MyInfo.gameObject.SetActive(true);
            ui_MyInfo.Open();
        }));
        SetHero(hero_id);
        btnNext.onClick.Add(new EventDelegate(() => {
            SoundEffectManager.effectSoundAction();
            if (hero_id < 5)
            {
                hero_id += 1;
            }else if (hero_id == 5)
            {
                hero_id = 1;
            }
            SetHero(hero_id);
        }));

        btnPrev.onClick.Add(new EventDelegate(() => {
            SoundEffectManager.effectSoundAction();
            if (hero_id > 1)
            {
                hero_id -= 1;
            }
            else if (hero_id == 1) 
            {
                hero_id = 5;
            }
            SetHero(hero_id);
        }));
    }
    private void SetHero(int hero_id)
    {
        if (heroGo != null)
        {
            Destroy(heroGo);
        }

        var data = DataManager.GetInstance().dicHeroData[hero_id];
        var prefab = (GameObject)(from obj in App.instance.resource
                                  where obj.name == data.prefab_name
                                  select obj).FirstOrDefault();
        heroGo = Instantiate(prefab);
        heroGo.transform.SetParent(heroPos.transform, false);

        SetLayersRecursively(heroGo.transform, "UI");

        heroGo.transform.rotation = Quaternion.Euler(0, 180, 0);

        heroGo.transform.localScale = heroScale;
        var anim = heroGo.GetComponentInChildren<Animator>();
        anim.Play($"Lobby_HeroStatus_Idle", -1, 0);

        
        lbSkills[0].text = DataManager.GetInstance().dicHeroStatusData[hero_id].skill1.ToString();

        lbSkills[1].text = DataManager.GetInstance().dicHeroStatusData[hero_id].skill2.ToString();

        lbSkills[2].text = DataManager.GetInstance().dicHeroStatusData[hero_id].skill3.ToString();


    }
    public void SetLayersRecursively(Transform trans, string name)
    {
        trans.gameObject.layer = LayerMask.NameToLayer(name);
        foreach (Transform child in trans)
        {
            SetLayersRecursively(child, name);
        }
    }
}
  
