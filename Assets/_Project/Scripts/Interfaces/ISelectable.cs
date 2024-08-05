using UnityEngine;
using UnityEngine.Events;

public interface ISelectable
{
    void OnSelected(out ICollector isValidTarget);
    bool IsSelectable { get; }
}

public interface ICollector
{
    UnityAction<ICollector> OnSuccess { get; set; }
    UnityAction<ICollector> OnFail { get; set; }
}

public interface ICollectable
{
    void Collect(ICollector collector, Vector3[] path, float duration);
}
