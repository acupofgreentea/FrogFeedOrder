using UnityEngine;

public interface IInteractableCell
{
    void Interact(ICellInteractable cellInteractable, out bool successfulInteraction);
    void DeInteract(ICellInteractable cellInteractable);
    public ContentColor GridColor { get; set; }
    GameObject gameObject { get; }
}