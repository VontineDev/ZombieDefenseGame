using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class App : MonoBehaviour
{
    public static App instance = null; 
    public GPGSLogin gpgsLogin;
    public Coroutine coScene;
    public System.Action OnCompleteScene;
    public Object[] resource;
    public int stageNumber;
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Screen.SetResolution(1080, 1920, true);

        DontDestroyOnLoad(this);
        this.resource = LoadResources();
        DataManager.GetInstance().LoadDatas();
        SceneManager.LoadSceneAsync("Logo").completed += (oper1) =>
        {
            var logo = GameObject.FindObjectOfType<Logo>();
            logo.OnLogoShown = () =>
            {
                SceneManager.LoadSceneAsync("Title").completed += (oper2) =>
                {
                    var title = GameObject.FindObjectOfType<TItle>();
                    Debug.LogFormat("Title: {0}", title);
                    title.OnTitleClicked = () =>
                    {
                        StartCoroutine(LoadAsync("Lobby2"));
                    };
                };
            };
        };


        OnCompleteScene = () =>
         {
             var sceneName = SceneManager.GetActiveScene().name;
             //Debug.Log($"sceneName:{sceneName}");
             switch (sceneName)
             {
                 case "Logo":
                     {

                     }
                     break;
                 case "Title":
                     {
                         ////StopCoroutine(coScene);
                         ////coScene = StartCoroutine(FlowScene());
                         //StartCoroutine(LoadAsync("Lobby2"));
                     }
                     break;
                 default:
                     {

                     }
                     break;
             }
         };
    }

    public Object[] LoadResources()
    {
        var resource = Resources.LoadAll("");
        //Debug.Log(resource[0].name);
        return resource;
    }
    IEnumerator LoadAsync(string SceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneName);
        var count = 0;
        while (!async.isDone)
        {
            yield return null;
            //Debug.Log(async.progress);
            count++;
            if (count > 100)
            {
                break;
            }
        }

    }

    IEnumerator FlowScene()
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            yield return null;
            // Debug.Log($"{time}");
            if (time > 0.1f)
            {
                //Debug.Log("1sec Passed");
                OnCompleteScene();
                break;
            }
        }
    }
}
