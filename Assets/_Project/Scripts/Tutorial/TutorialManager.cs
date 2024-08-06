using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [field: SerializeField] public Tutorial CurrentTutorial { get; private set; }
    
    public static TutorialManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    public void StartTutorial(Tutorial tutorial)
    {
        CurrentTutorial = tutorial;
        CurrentTutorial.StartTutorial();
    }
    
    public void EndCurrentTutorial()
    {
        CurrentTutorial.EndTutorial();
        CurrentTutorial = null;
    }
}