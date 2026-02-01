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
    private float originalForwardSpeed;
    private float originalrSpeed;

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
        originalForwardSpeed = forwardSpeed;
        originalrSpeed = rSpeed;
    }


    private void Update()
    {
        transform.Rotate(0, currDir.x * rSpeed, 0);
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = new Vector3((transform.forward* forwardSpeed).x,_rb.linearVelocity.y, (transform.forward * forwardSpeed).z);

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
        forwardSpeed = Mathf.Min(originalForwardSpeed + (DifficultyManager.SpeedMultiplier) * 2f, 30);
        rSpeed = Mathf.Min(originalrSpeed + (DifficultyManager.SpeedMultiplier), 15);
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
