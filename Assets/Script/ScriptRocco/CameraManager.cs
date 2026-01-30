using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    void Start()
    {
        transform.position = target.position + offset;
    }
    void Update()
    {
        transform.position = target.position + offset;
        transform.forward = target.forward;
        transform.LookAt(target);
    }
}
