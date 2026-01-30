using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Square : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float jumpForce = 7.0f;

    //InputManager inputManager;

    Rigidbody _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

 
    void Update()
    {
        
    }


}
