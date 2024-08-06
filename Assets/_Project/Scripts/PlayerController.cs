using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private Camera mainCam;
    public static event UnityAction<int> OnRemainingMovesUpdated;
    public static event UnityAction<bool, ICollector> OnCollecterMoved;
    
    public static event UnityAction<ISelectable> OnSelectableSelected;
    public static event UnityAction OnRemaningMovesFinished;    
    private int remaningMoves = 5;
    private int fakeRemainingMoves;


    private void Awake()
    {
        mainCam = Camera.main;
        LevelManager.OnLevelLoaded += HandleLevelLoaded;
    }

    private void HandleLevelLoaded(LevelDataSO levelDataSo)
    {
        remaningMoves = levelDataSo.MoveCount;
        fakeRemainingMoves = remaningMoves;
        OnRemainingMovesUpdated?.Invoke(remaningMoves);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        if (remaningMoves == 0)
            return;

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out ISelectable selectable))
            {
                if (!selectable.IsSelectable)
                    return;
                
                remaningMoves--;
                OnSelectableSelected?.Invoke(selectable);
                OnRemainingMovesUpdated?.Invoke(remaningMoves);

                selectable.OnSelected(out ICollector collector);

                if (collector != null)
                {
                    collector.OnSuccess += HandleOnSuccess;
                    collector.OnFail += HandleOnFail;
                }
            }
        }
    }

    private void HandleOnFail(ICollector collector)
    {
        OnCollecterMoved?.Invoke(false, collector);
        collector.OnSuccess -= HandleOnSuccess;
        collector.OnFail -= HandleOnFail;
        
        UpdateRemainingMoves();
    }

    private void HandleOnSuccess(ICollector collector)
    {
        OnCollecterMoved?.Invoke(true, collector);
        collector.OnSuccess -= HandleOnSuccess;
        collector.OnFail -= HandleOnFail;
        
        UpdateRemainingMoves();
    }

    private void UpdateRemainingMoves()
    {
        fakeRemainingMoves--;
        if (fakeRemainingMoves == 0)
        {
            OnRemaningMovesFinished?.Invoke();
            return;
        }
    }
}