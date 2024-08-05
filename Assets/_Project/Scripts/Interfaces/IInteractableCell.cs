using UnityEngine;

public interface IInteractableCell
{
    void Interact(ICellInteractable cellInteractable);
    void DeInteract(ICellInteractable cellInteractable);
    public ContentColor GridColor { get; set; }
    GameObject gameObject { get; }
}