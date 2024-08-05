using UnityEngine;

[CreateAssetMenu(fileName = "GridCellPrefabHolder", menuName = "GridCellPrefabHolder", order = 0)]
public class GridCellPrefabHolder : ScriptableObject
{
    [SerializeField] private GridCellBase frogPrefab;
    [SerializeField] private GridCellBase grapePrefab;
    [SerializeField] private GridCellBase emptyPrefab;
    [SerializeField] private GridCellBase directionChangerPrefab;

    public GridCellBase GetPrefabByType(GridState type)
    {
        switch (type)
        {
            case GridState.Frog:
                return frogPrefab;
            case GridState.Grape:
                return grapePrefab;
            case GridState.Empty:
                return emptyPrefab;
            case GridState.DirectionChanger:
                return directionChangerPrefab;
            default:
                return emptyPrefab;
        }
    }
}