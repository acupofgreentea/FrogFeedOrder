using UnityEngine;

public static class Helpers
{
    public static Vector3 GetDirectionVector(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Vector3.forward,
            Direction.Down => Vector3.back,
            Direction.Left => Vector3.left,
            Direction.Right => Vector3.right,
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
}