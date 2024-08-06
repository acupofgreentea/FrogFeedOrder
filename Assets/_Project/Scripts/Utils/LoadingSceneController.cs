using UnityEngine;

public class LoadingSceneController : MonoBehaviour
{
    private void Start()
    {
        SceneController.Instance.LoadSceneAsync(Constants.GAME_SCENE_INDEX);
    }
}
