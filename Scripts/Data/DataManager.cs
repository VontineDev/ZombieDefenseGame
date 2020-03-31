
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System;

public class DataManager
{
    private static DataManager Instance;

    public static DataManager GetInstance()
    {
        if (DataManager.Instance == null)
        {
            DataManager.Instance = new DataManager();
        }
        return DataManager.Instance;
    }

    public Dictionary<int, CommanderData> dicCommanderData;
    public Dictionary<int, HeroData> dicHeroData;
    public Dictionary<int, ItemData> dicItemData;
    public Dictionary<int, ItemInfo> dicItemInfo;
    //public Dictionary<int, SkillData> dicSkillData;
    public Dictionary<int, DefenseMonsterData> dicMonsterData;
    public Dictionary<int, FenceData> dicFenceData;
    public Dictionary<int, StageData> dicStageData;
    public Dictionary<int, WaveData> dicWaveData;
    public Dictionary<int, FencePositionData> dicFencePositionData;
    public Dictionary<int, CommanderPositionData> dicCommanderPositionData;
    public Dictionary<int, HeroPositionData> dicHeroPositionData;
    public Dictionary<int, CameraPositionData> dicCameraPositionData;

    public Dictionary<int, CommanderInfo> dicCommanderinfo;
    public Dictionary<int, UpgradeData> dicUpgradeData;
    public Dictionary<int, AchievementsData> dicAchievementsData;
    public Dictionary<int, HeroStatusData> dicHeroStatusData;

    public UserInfo userInfo;
    public Dictionary<int, StageClearInfo> dicStageClearInfo;
    public Dictionary<int, AchievementsInfo> dicAchievementsInfo;
    //public Dictionary<int,CommanderInfo> dicCommanderInfo;
    public const ulong MAX_CASH = 999999999999999;
    public void LoadDatas()
    {
        LoadCommanderData();
        LoadHeroData();
        LoadItemData();
        LoadItemInfo();
        //LoadSkillData();
        LoadDefenseMonsterData();
        LoadFenceData();
        LoadStageData();
        LoadWaveData();
        LoadFencePositionData();
        LoadCommanderPositionData();
        LoadHeroPositionData();
        LoadCameraPositionData();
        LoadCommanderinfo();
        LoadUserInfo();
        LoadUpgradeData();
        LoadAchievementsData();
        LoadHeroStatusData();
        LoadStageClearInfo();
        LoadAchievementsInfo();
    }

    #region ITEM_DATA
    private void LoadItemData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "item_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicItemData = new Dictionary<int, ItemData>();
        var arr = JsonConvert.DeserializeObject<ItemData[]>(json);

        foreach (var data in arr)
        {
            dicItemData.Add(data.id, data);
        }
    }
    #endregion

    #region FENCE_DATA
    private void LoadFenceData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "fence_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicFenceData = new Dictionary<int, FenceData>();
        var arr = JsonConvert.DeserializeObject<FenceData[]>(json);

        foreach (var data in arr)
        {
            dicFenceData.Add(data.id, data);
        }
    }
    #endregion

    #region COMMANDER_DATA
    private void LoadCommanderData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "commander_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicCommanderData = new Dictionary<int, CommanderData>();
        var arr = JsonConvert.DeserializeObject<CommanderData[]>(json);

        foreach (var data in arr)
        {
            dicCommanderData.Add(data.id, data);
        }
    }
    #endregion

    #region HERO_DATA
    private void LoadHeroData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "hero_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicHeroData = new Dictionary<int, HeroData>();
        var arr = JsonConvert.DeserializeObject<HeroData[]>(json);

        foreach (var data in arr)
        {
            dicHeroData.Add(data.id, data);
        }

    }
    #endregion

    #region DEFENSEMONSTER_DATA
    private void LoadDefenseMonsterData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "defense_monster_data")
            {
                json = data.ToString();

                break;
            }
        }

        dicMonsterData = new Dictionary<int, DefenseMonsterData>();
        var arr = JsonConvert.DeserializeObject<DefenseMonsterData[]>(json);

        foreach (var data in arr)
        {
            dicMonsterData.Add(data.id, data);
            //Debug.Log(data.id);
        }

    }
    #endregion

    #region STAGE_DATA
    private void LoadStageData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "stage_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicStageData = new Dictionary<int, StageData>();
        var arr = JsonConvert.DeserializeObject<StageData[]>(json);

        foreach (var data in arr)
        {
            dicStageData.Add(data.id, data);
        }
    }
    #endregion

    #region UPGRADE_DATA
    private void LoadUpgradeData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "upgrade_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicUpgradeData = new Dictionary<int, UpgradeData>();
        var arr = JsonConvert.DeserializeObject<UpgradeData[]>(json);

        foreach (var data in arr)
        {
            dicUpgradeData.Add(data.id, data);
        }
    }
    #endregion

    #region HEROSTATUS_DATA
    private void LoadHeroStatusData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "hero_status_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicHeroStatusData = new Dictionary<int, HeroStatusData>();
        var arr = JsonConvert.DeserializeObject<HeroStatusData[]>(json);

        foreach (var data in arr)
        {
            dicHeroStatusData.Add(data.id, data);
        }
    }
    #endregion

    #region ACHIEVEMENTS_DATA
    private void LoadAchievementsData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "achievements_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicAchievementsData = new Dictionary<int, AchievementsData>();
        var arr = JsonConvert.DeserializeObject<AchievementsData[]>(json);

        foreach (var data in arr)
        {
            dicAchievementsData.Add(data.id, data);
        }
    }
    #endregion

    #region WAVE_DATA
    private void LoadWaveData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "wave_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicWaveData = new Dictionary<int, WaveData>();

        var arr = JsonConvert.DeserializeObject<WaveData[]>(json);

        foreach (var data in arr)
        {
            dicWaveData.Add(data.id, data);
        }
    }
    #endregion

    #region POSITION_DATA
    private void LoadFencePositionData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "fence_position_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicFencePositionData = new Dictionary<int, FencePositionData>();

        var arr = JsonConvert.DeserializeObject<FencePositionData[]>(json);

        foreach (var data in arr)
        {
            dicFencePositionData.Add(data.id, data);
        }

    }
    private void LoadCommanderPositionData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "commander_position_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicCommanderPositionData = new Dictionary<int, CommanderPositionData>();
        var arr = JsonConvert.DeserializeObject<CommanderPositionData[]>(json);

        foreach (var data in arr)
        {
            dicCommanderPositionData.Add(data.id, data);
        }

    }
    private void LoadHeroPositionData()
    {
        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "hero_position_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicHeroPositionData = new Dictionary<int, HeroPositionData>();
        var arr = JsonConvert.DeserializeObject<HeroPositionData[]>(json);

        foreach (var data in arr)
        {
            dicHeroPositionData.Add(data.id, data);
        }

    }
    private void LoadCameraPositionData()
    {

        string json = null;
        foreach (var data in App.instance.resource)
        {
            if (data.name == "camera_position_data")
            {
                json = data.ToString();

                break;
            }
        }
        dicCameraPositionData = new Dictionary<int, CameraPositionData>();

        var arr = JsonConvert.DeserializeObject<CameraPositionData[]>(json);

        foreach (var data in arr)
        {
            dicCameraPositionData.Add(data.id, data);
        }
    }
    #endregion

    #region USER_INFO
    private void LoadUserInfo()
    {
        string path = $"{Application.persistentDataPath}/user_info.json";
        string json = null;

        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            userInfo = JsonConvert.DeserializeObject<UserInfo>(json);
        }
        else
        {
            Debug.Log("저장된 user_info 파일이 없습니다. 새로운 파일을 만듭니다");

            foreach (var data in App.instance.resource)
            {
                if (data.name == "user_info")
                {
                    json = data.ToString();
                    var arr = JsonConvert.DeserializeObject<UserInfo[]>(json);
                    userInfo = arr[0];
                    break;
                }
            }
        }


    }
    public void SaveUserInfo()
    {
        CheckMaxCash();

        string path = $"{Application.persistentDataPath}/user_info.json";

        string json = JsonConvert.SerializeObject(userInfo);

        File.WriteAllText(path, json);
    }
    public void AddCash(ulong cashToAdd)
    {
        this.userInfo.cash += cashToAdd;
    }
    public void SetUId(string str)
    {
        userInfo.uId = str;
    }

    public void AddMonsterKiilCount()
    {
        this.userInfo.monsterKillCount++;
    }
    #endregion

    #region ITEM_INFO
    private void LoadItemInfo()
    {
        string path = $"{Application.persistentDataPath}/item_info.json";
        string json = null;
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }
        else
        {
            Debug.Log("저장된 item_info 파일이 없습니다. 새로운 파일을 만듭니다");

            foreach (var data in App.instance.resource)
            {
                if (data.name == "item_info")
                {
                    json = data.ToString();

                    break;
                }
            }
        }
        dicItemInfo = new Dictionary<int, ItemInfo>();

        var arr = JsonConvert.DeserializeObject<ItemInfo[]>(json);

        foreach (var data in arr)
        {
            dicItemInfo.Add(data.id, data);
        }
    }
    public void SaveItemInfo()
    {
        string path = $"{Application.persistentDataPath}/item_info.json";

        var arr = new ItemInfo[dicItemInfo.Count];
        int i = 0;
        foreach (var itemInfo in dicItemInfo)
        {
            arr[i] = itemInfo.Value;
            i++;
        }

        string json = JsonConvert.SerializeObject(arr);

        File.WriteAllText(path, json);
    }
    public void UnEquipItem(int id)
    {
        dicItemInfo[id].isEquipped = false;
    }
    public void EquipItem(int id)
    {
        dicItemInfo[id].isEquipped = true;
    }
    public void BuyItem(int id)
    {
        dicItemInfo[id].isPossessed = true;
    }
    #endregion

    #region COMMANDER_INFO
    private void LoadCommanderinfo()
    {
        string path = $"{Application.persistentDataPath}/commander_info.json";
        string json = null;
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }
        else
        {
            Debug.Log("저장된 commander_info 파일이 없습니다. 새로운 파일을 만듭니다");

            foreach (var data in App.instance.resource)
            {
                if (data.name == "commander_info")
                {
                    json = data.ToString();

                    break;
                }
            }
        }
        dicCommanderinfo = new Dictionary<int, CommanderInfo>();

        var arr = JsonConvert.DeserializeObject<CommanderInfo[]>(json);

        foreach (var data in arr)
        {
            dicCommanderinfo.Add(data.id, data);
        }
        SetInfoFromLv();
    }
    public void SetInfoFromLv()
    {
        foreach (var kv in dicCommanderinfo)
        {
            var data = dicCommanderData[kv.Value.commander_id];
            kv.Value.damageTotal = FormulaCalculator.GetInstance().CalculateDamage(data.damage, kv.Value.damageLv);
            kv.Value.atkSpeedTotal = FormulaCalculator.GetInstance().CalculateAspd(data.atkSpeed, kv.Value.atkSpeedLV);
        }
    }

    public void SaveCommanderInfo()
    {
        string path = $"{Application.persistentDataPath}/commander_info.json";

        var arr = new CommanderInfo[dicCommanderinfo.Count];
        int i = 0;
        foreach (var commanderInfo in dicCommanderinfo)
        {
            arr[i] = commanderInfo.Value;
            i++;
        }

        string json = JsonConvert.SerializeObject(arr);

        File.WriteAllText(path, json);
    }
    #endregion    

    #region STAGECLEAR_INFO
    private void LoadStageClearInfo()
    {
        string path = $"{Application.persistentDataPath}/stageClear_info.json";
        string json = null;
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }
        else
        {
            Debug.Log("저장된 stageClear_info 파일이 없습니다. 새로운 파일을 만듭니다");

            foreach (var data in App.instance.resource)
            {
                if (data.name == "stageClear_info")
                {
                    json = data.ToString();

                    break;
                }
            }
        }
        dicStageClearInfo = new Dictionary<int, StageClearInfo>();

        var arr = JsonConvert.DeserializeObject<StageClearInfo[]>(json);

        foreach (var data in arr)
        {
            dicStageClearInfo.Add(data.id, data);
        }
    }
    public void SetStageCleared(int stageId)
    {
        if (0 < stageId && stageId < 17)
        {
            dicStageClearInfo[stageId].isCleared = true;
            if (stageId < 16)
            {
                dicStageClearInfo[stageId + 1].isLocked = false;
            }
        }

    }
    public void SaveStageClearInfo()
    {
        string path = $"{Application.persistentDataPath}/stageClear_info.json";

        var arr = new StageClearInfo[dicStageClearInfo.Count];
        int i = 0;
        foreach (var info in dicStageClearInfo)
        {
            arr[i] = info.Value;
            i++;
        }
        string json = JsonConvert.SerializeObject(arr);

        File.WriteAllText(path, json);
    }
    #endregion

    #region ACHIEVEMENTS_INFO
    private void LoadAchievementsInfo()
    {
        string path = $"{Application.persistentDataPath}/achievements_info.json";
        string json = null;
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
        }
        else
        {
            Debug.Log("저장된 achievements_info 파일이 없습니다. 새로운 파일을 만듭니다");

            foreach (var data in App.instance.resource)
            {
                if (data.name == "achievements_info")
                {
                    json = data.ToString();
                    break;
                }
            }
        }
        dicAchievementsInfo = new Dictionary<int, AchievementsInfo>();

        var arr = JsonConvert.DeserializeObject<AchievementsInfo[]>(json);

        foreach (var data in arr)
        {
            dicAchievementsInfo.Add(data.id, data);
        }
    }
    public void SetStageAchivementCleared(int stageId)
    {
        dicAchievementsInfo[stageId].isCleared = true;
        SaveAchievementsInfo();
    }
    public void SetMonsterKillCountAchivementCleared()
    {
        var monsterKillCount = userInfo.monsterKillCount;

        foreach (var kv in dicAchievementsInfo)
        {
            if (kv.Value.type == 2)
            {
                if (monsterKillCount >= dicAchievementsData[kv.Key].goal)
                {
                    kv.Value.isCleared = true;
                }
            }
        }
        SaveAchievementsInfo();
    }
    public void SaveAchievementsInfo()
    {
        string path = $"{Application.persistentDataPath}/achievements_info.json";

        var arr = new AchievementsInfo[dicAchievementsInfo.Count];
        int i = 0;
        foreach (var info in dicAchievementsInfo)
        {
            arr[i] = info.Value;
            i++;
        }

        string json = JsonConvert.SerializeObject(arr);

        File.WriteAllText(path, json);
    }
    #endregion

    public void CheckMaxCash()
    {
        if (this.userInfo.cash > MAX_CASH)
        {
            this.userInfo.cash = MAX_CASH;
        }
    }
}
