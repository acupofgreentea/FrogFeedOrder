using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 1f;
    
    [SerializeField] private Vector3 rotationAxis = Vector3.forward;
    
    private void Update()
    {
        target.Rotate(rotationAxis, speed * Time.deltaTime);
    }
}
