using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }
    
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnSelected();
            }
        }
    }
}