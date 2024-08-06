using UnityEngine;

public interface ICollectable
{
    void Collect(ICollector collector, Vector3[] path, float duration);
}