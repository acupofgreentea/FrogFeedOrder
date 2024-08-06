using UnityEngine.Events;

public interface ICollector
{
    UnityAction<ICollector> OnSuccess { get; set; }
    UnityAction<ICollector> OnFail { get; set; }
}