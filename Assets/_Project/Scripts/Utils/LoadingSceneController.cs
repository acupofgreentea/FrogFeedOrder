using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    private void Start()
    {
        LoadGameScene();
    }
    
    private void LoadGameScene()
    {
        var asyncOperation = SceneManager.LoadSceneAsync(Constants.GAME_SCENE_INDEX);
        asyncOperation.allowSceneActivation = false;

        StartCoroutine(DelayedActivation());
        IEnumerator DelayedActivation()
        {
            yield return new WaitForSeconds(1f);
            asyncOperation.allowSceneActivation = true;
        }
    }
}
