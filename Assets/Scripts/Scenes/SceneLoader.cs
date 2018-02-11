using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A singleton class for managing the loading/unloading of scenes
/// data, etc.
/// </summary>
public class SceneLoader : MonoBehaviour {

    // the psuedo-singleton instance holding the value created by unity
    public static SceneLoader Instance { get; private set; }    

    private string currentScene;

    void Awake()
    {
        // assign this object created by unity
        Instance = this;
        
        // If there is only 1 scene loaded (this one) then load the Login screen
        if(UnityEngine.SceneManagement.SceneManager.sceneCount == 1)
        {
            SwitchScenes("Level1");
        }

        // If there are other scenes loaded (usually while in development) then
        // make sure we know which is the current scene loaded so that we can 
        // unload it later
        else
        {
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; ++i)
            {
                Scene sc = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                if (sc.name != "Global")
                {
                    currentScene = sc.name;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Switches to a target scene
    /// </summary>
    /// <param name="toScene"></param>
    public void SwitchScenes(string toScene)
    {
        StartCoroutine(SceneSwitcher(toScene));
    }

    /// <summary>
    /// Switches to target scene asynchronously
    /// </summary>
    /// <param name="toScene"></param>
    /// <returns></returns>
    IEnumerator SceneSwitcher(string toScene)
    {
        Debug.Log("Switching scenes: " + currentScene + " -> " + toScene);

        yield return new WaitForSeconds(.1f);
        if (currentScene != null)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
        }

        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        UnityEngine.SceneManagement.SceneManager.LoadScene(toScene, LoadSceneMode.Additive);
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        currentScene = arg0.name;
    }    
}