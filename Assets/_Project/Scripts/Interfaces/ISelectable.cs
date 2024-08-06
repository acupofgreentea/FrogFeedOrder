public interface ISelectable
{
    void OnSelected(out ICollector isValidTarget);
    bool IsSelectable { get; }
}