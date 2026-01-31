using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Square : MonoBehaviour
{
    [SerializeField] private float rSpeed = 2.0f;
    [SerializeField] private float forwardSpeed = 5.0f;
    [SerializeField] private float jumpForce = 7.0f;

    [SerializeField] private PlayerSFXManager playerSfxManager;

    private Vector2 currDir;

    Rigidbody _rb;

    void Awake()
    {
        EntrySquarePoint.OnPlayerEnterOnEntryPoint += HandleEntryOnSquare;

        ExitSquarePoint.OnPlayerEnterOnExitPoint += HandleExitOnSquare;
    }

    void Start()
    {
        InputManager.OnPlayerMovement += HandlePlayerInput;
        _rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        transform.Rotate(0, currDir.x * rSpeed, 0);
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = transform.forward * forwardSpeed;
        //_rb.linearVelocity = new Vector3(currDir.x * speed, 0 , forwardSpeed);
        //_rb.transform.forward = new Vector3(currDir.x * speed, 0, forwardSpeed).normalized;

    }
    private void OnDestroy()
    {
        InputManager.OnPlayerMovement -= HandlePlayerInput;

        EntrySquarePoint.OnPlayerEnterOnEntryPoint -= HandleEntryOnSquare;

        ExitSquarePoint.OnPlayerEnterOnExitPoint -= HandleExitOnSquare;
    }

    private void HandleEntryOnSquare()
    {
        this.enabled = true;
    }

    private void HandleExitOnSquare()
    {
        this.enabled = false;
    }

    private void HandlePlayerInput(Vector2 dir)
    {
        currDir = dir;
    }

}
