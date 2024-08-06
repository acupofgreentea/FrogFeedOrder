using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

 
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(int sceneIndex, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
    }

    public void LoadSceneAsync(int sceneIndex, float totalTimeAtLeast, UnityAction onStart = null, UnityAction onComplete = null,
        LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        onStart?.Invoke();
        var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, loadSceneMode);
        asyncOperation.allowSceneActivation = false;

        StartCoroutine(DelayedActivation());
        IEnumerator DelayedActivation()
        {
            yield return new WaitForSeconds(totalTimeAtLeast);
            asyncOperation.allowSceneActivation = true;
            
            onComplete?.Invoke();
        }
    }

}