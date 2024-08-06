using System.Collections.Generic;
using UnityEngine;

public class FrogTutorial : Tutorial
{
    private int leftFrogsCount;
    private const string tutorialText = "Click on Frog to Feed!";
    private void Start()
    {
        if (DataManager.CurrentLevel == 0) //first level
            TutorialManager.Instance.StartTutorial(this);
    }

    public override void StartTutorial()
    {
        PlayerController.OnSelectableSelected += HandleSelectableSelected;
        leftFrogsCount = LevelManager.Instance.GetActiveFrogs().Count;
        
        var frogs = LevelManager.Instance.GetActiveFrogs();
        
        foreach (Frog frog in frogs)
        {
            frog.IsSelectable = false;
        }
        
        SelectFrog(frogs);
    }

    private void SelectFrog(List<Frog> frogs)
    {
        var currentFrog = frogs[leftFrogsCount - 1];
        currentFrog.IsSelectable = true;
        
        TutorialUI.UpdateTutorialUI?.Invoke(true, currentFrog.transform.position, tutorialText);
    }

    private void HandleSelectableSelected(ISelectable selectedFrog)
    {
        if (selectedFrog is not Frog)
            return;
        
        leftFrogsCount--;
        
        if (leftFrogsCount == 0)
            TutorialManager.Instance.EndCurrentTutorial();
        else
        {
            SelectFrog(LevelManager.Instance.GetActiveFrogs());
        }
    }

    public override void EndTutorial()
    {
        TutorialUI.UpdateTutorialUI?.Invoke(false, Vector3.zero, "");
        PlayerController.OnSelectableSelected -= HandleSelectableSelected;
    }
}