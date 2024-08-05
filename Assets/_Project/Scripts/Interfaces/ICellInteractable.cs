using UnityEngine;

public interface ICellInteractable
{
    ContentColor ContentColor { get; }
    Direction Direction { get; set; }
    GameObject gameObject { get; }
}