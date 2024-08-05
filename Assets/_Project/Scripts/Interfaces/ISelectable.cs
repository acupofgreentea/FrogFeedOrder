using UnityEngine.Events;

public interface ISelectable
{
    void OnSelected(out ICollector isValidTarget);
    bool IsSelectable { get; }
}

public interface ICollector : ISelectable
{
    UnityAction<ICollector> OnSuccess { get; set; }
    UnityAction<ICollector> OnFail { get; set; }
}
