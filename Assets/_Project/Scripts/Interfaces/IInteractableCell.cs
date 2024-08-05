public interface IInteractableCell
{
    void Interact(ICellInteractable cellInteractable);
    public ContentColor GridColor { get; set; }

}