using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
public class TestSaveData : MonoBehaviour
{
    public string path;
    private UserInfo userInfo;

    public UIButton btnLoad;
    public UIButton btnChangeInfo;
    public UIButton btnSave;

    public UILabel uId;
    public UILabel userName;
    public UILabel heroDamageLv;
    public UILabel heroAtkSpeedLv;

    void Start()
    {
        path = $"{Application.persistentDataPath}/user_info.json";
        Debug.Log(path);

        this.btnLoad.onClick.Add(new EventDelegate(() =>
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                userInfo = JsonConvert.DeserializeObject<UserInfo>(json);
            }
            else
            {
                userInfo = new UserInfo();
                Debug.Log("저장된 파일이 없습니다");
            }
            ShowInfo();
        }));

        this.btnChangeInfo.onClick.Add(new EventDelegate(() =>
        {
            ChangeInfo();
        }));

        this.btnSave.onClick.Add(new EventDelegate(() =>
        {
            string json = JsonConvert.SerializeObject(userInfo);

            File.WriteAllText(path, json);
        }));

    }
    public void GetMessage()
    {

    }
    private void ShowInfo()
    {
        uId.text = userInfo.uId;
        userName.text = userInfo.userName;
        heroDamageLv.text = userInfo.heroDamageLv.ToString();
        heroAtkSpeedLv.text = userInfo.heroAtkSpeedLv.ToString();
    }
    private void ChangeInfo()
    {
        userInfo.uId = uId.text;
        userInfo.userName = userName.text;
        
        if(int.TryParse(heroDamageLv.text, out userInfo.heroDamageLv))
        {
            Debug.Log("heroDamageLv 정상입력");
        }
        else
        {
            Debug.Log("heroDamageLv 비정상");
        }
        if (int.TryParse(heroAtkSpeedLv.text, out userInfo.heroAtkSpeedLv))
        {
            Debug.Log("heroAtkSpeedLv 정상입력");
        }
        else
        {
            Debug.Log("heroAtkSpeedLv 비정상");
        }
    }
}
