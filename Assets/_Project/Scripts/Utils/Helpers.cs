using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Helpers
{
    public static Vector3 GetDirectionVector(Direction direction, float offset = 0.5f)
    {
        return direction switch
        {
            Direction.Up => Vector3.forward,
            Direction.Down => Vector3.back,
            Direction.Left => Vector3.left,
            Direction.Right => Vector3.right,
            Direction.Above => Vector3.up * offset,
            _ => Vector3.zero
        };
    }

    public static Direction GetOppositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => Direction.Up
        };
    }
    
    public static Quaternion GetRotationByDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Quaternion.Euler(0, 180, 0);
            case Direction.Down:
                return Quaternion.Euler(0, 0, 0);
            case Direction.Right:
                return Quaternion.Euler(0, -90, 0);
            case Direction.Left:
                return Quaternion.Euler(0, 90, 0);
            default:
                return Quaternion.identity;
        }
    }

    public static Vector3 GetOppositeDirectionVector(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Vector3.back,
            Direction.Down => Vector3.forward,
            Direction.Left => Vector3.right,
            Direction.Right => Vector3.left,
            _ => Vector3.zero
        };
    }
    
    public static Vector3[] GetPath(GameObject[] gos)
    {
        Vector3[] path = new Vector3[gos.Length];
        for (int i = 0; i < gos.Length; i++)
        {
            path[i] = gos[i].transform.position;
        }
        return path;
    }

    public static Vector3[] GetPath(List<IInteractableCell> movedInteractableCells)
    {
        GameObject[] moved = new GameObject[movedInteractableCells.Count];
        for (var i = 0; i < movedInteractableCells.Count; i++)
        {
            var movedInteractableCell = movedInteractableCells[i];
            moved[i] = movedInteractableCell.gameObject;
        }

        moved = moved.Reverse().ToArray();
        return GetPath(moved);
    }

#if UNITY_EDITOR

    public static T FindObject<T>() where T : ScriptableObject
    {
        string typeName = typeof(T).Name;

        string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeName}");

        if (guids.Length == 0)
        {
            Debug.LogWarning($"No asset of type {typeName} found.");
            return null;
        }

        if (guids.Length > 1)
        {
            Debug.LogWarning($"Multiple assets of type {typeName} found. Returning the first one.");
        }

        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
        T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);

        if (asset == null)
        {
            Debug.LogError($"Failed to load asset at path: {path}");
        }

        return asset;
    }
#endif

}