using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CFSceneLoader : MonoBehaviour
{
    public List<string> alwaysLoadList;
    public bool unload;

    private void Start()
    {
        unload = false;
    }

    public void LoadSceneSingle(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadScene(string sceneName)
    {
        foreach(string name in alwaysLoadList)
        {
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        }
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        unload = true;
    }

    private void Update()
    {
        if (unload)
            SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
