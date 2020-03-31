using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader
{
    public static SceneLoader Instance;

    public static SceneLoader GetInstance()
    {
        if (SceneLoader.Instance == null)
        {
            SceneLoader.Instance = new SceneLoader();
        }
        return SceneLoader.Instance;
    }
    public void LoadSingleScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);       
    }
    public void LoadAdditiveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }  
}
