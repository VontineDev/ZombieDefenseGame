using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Lobby : MonoBehaviour
{
    public Vector3 commanderRotation;
    public Vector3 commaderPosition;

    public GameObject commanderGo;
    public UI_LobbyButtons uI_LobbyButtons;

    void Start()
    {
        commanderRotation = new Vector3(0, 180, 0);
        commaderPosition = new Vector3(0, 0, -5.4f);
        SetTimeScale(1f);

        uI_LobbyButtons.OnSetCommObjCallback = (commander_id) =>
        {
            SetCommander(commander_id);
        };

    }

    private void SetTimeScale(float num)
    {
        Time.timeScale = num;
    }

    private void SetCommander(int commander_id)
    {
        if (commanderGo != null)
        {
            Destroy(commanderGo);
        }

        var data = DataManager.GetInstance().dicCommanderData[commander_id];
        UnityEngine.Object prefab = (from obj in App.instance.resource
                                     where obj.name == data.prefab_name
                                     select obj).FirstOrDefault();
        commanderGo = (GameObject)Instantiate(prefab);
        commanderGo.transform.position = commaderPosition;
        commanderGo.transform.rotation = Quaternion.Euler(0, 180, 0);

        var anim = commanderGo.GetComponentInChildren<Animator>();
        anim.Play($"{data.prefab_name}_Idle");

        SetCommanderInfo(commander_id);
    }

    private void SetCommanderInfo(int commander_id)
    {
        var commInfo = DataManager.GetInstance().dicCommanderinfo;
        foreach (var info in commInfo)
        {
            info.Value.isSelected = false;
        }
        commInfo[commander_id].isSelected = true;
    }
}
