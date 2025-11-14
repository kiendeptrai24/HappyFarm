using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoadManager : Singleton<SceneLoadManager>
{
    protected void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadRegularScene(string sceneName, bool useLoadScreen = true)
    {
        StartCoroutine(ProcessRegularSceneLoading(sceneName, useLoadScreen));
    }

    private IEnumerator ProcessRegularSceneLoading(string sceneToLoad, bool useLoadScene = true)
    {
        if (useLoadScene)
        {
            SceneManager.LoadScene("LoadingScene");
            yield return new WaitForSeconds(1f);
        }
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
    }
}