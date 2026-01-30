using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Square : MonoBehaviour
{
    [SerializeField] private float rSpeed = 5.0f;
    [SerializeField] private float forwardSpeed = 5.0f;
    [SerializeField] private float jumpForce = 7.0f;
   

    private Vector2 currDir;

    Rigidbody _rb;


    void Start()
    {
        InputManager.OnPlayerMovement += HandlePlayerInput;
        _rb = GetComponent<Rigidbody>();
    }

 
    void Update()
    {

        _rb.linearVelocity = transform.forward * forwardSpeed;
        _rb.transform.Rotate(0, currDir.x * rSpeed, 0);

        //_rb.linearVelocity = new Vector3(currDir.x * speed, 0 , forwardSpeed);
        //_rb.transform.forward = new Vector3(currDir.x * speed, 0, forwardSpeed).normalized;

    }
    private void OnDestroy()
    {
        InputManager.OnPlayerMovement -= HandlePlayerInput;
    }

    private void HandlePlayerInput(Vector2 dir)
    {
        currDir = dir;
    }

}
