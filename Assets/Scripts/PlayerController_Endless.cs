using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerController_Endless : MonoBehaviour
{
    [SerializeField] private float laneOffset = 2.5f;
    [SerializeField] private float laneChangeSpeed = 10f;
    [SerializeField] private float yOffset = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float fallMultiplier = 300f;
    [SerializeField] private float lowJumpMultiplier = 50f;
    [SerializeField] private float healTime = 5f; // time to heal back to full hp
    [SerializeField] private float knockbackDistance = 4f;
    [SerializeField] private float knockbackSpeed = 10f;
    [SerializeField] private float returnSpeed = 5f;

    [SerializeField] private PlayerSFXManager playerSfxManager;

    private Rigidbody rb;
    private bool isGrounded;
    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    private Vector3 targetPosition;
    private Vector3 originPoint;
    private int hp = 2; // va pi� lento ad un hp, ma pu� recuperarlo.
    private bool isHurt = false;
    private float healTimer = 0f;
    private float baseZ;
    private float targetZ;
    private bool returning = false;


    public Action OnPlayerDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        InputManager.OnPlayerMovement += HandlePlayerInput;
        InputManager.OnPlayerJump += HandlePlayerJump;

        EntrySquarePoint.OnPlayerEnterOnEntryPoint += HandleEnterOnSquare;

        ExitSquarePoint.OnPlayerEnterOnExitPoint += HandleExitOnSquare;

        targetPosition = transform.position;
        originPoint = transform.forward;

        baseZ = transform.position.z;
        targetZ = baseZ;
    }

    void OnDestroy()
    {
        InputManager.OnPlayerMovement -= HandlePlayerInput;
        InputManager.OnPlayerJump -= HandlePlayerJump;

        EntrySquarePoint.OnPlayerEnterOnEntryPoint -= HandleEnterOnSquare;

        ExitSquarePoint.OnPlayerEnterOnExitPoint -= HandleExitOnSquare;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(
            pos.x,
            targetPosition.x,
            laneChangeSpeed * Time.deltaTime
        );
        transform.position = pos;

        // HURT
        if (isHurt)
        {
            playerSfxManager.PlayHurtSFX();

            float speed = returning ? returnSpeed : knockbackSpeed;

            Vector3 curr_pos = transform.position;
            curr_pos.z = Mathf.Lerp(curr_pos.z, targetZ, speed * Time.deltaTime);
            transform.position = curr_pos;

            healTimer -= Time.deltaTime;

            if (healTimer <= 0f)
            {
                Debug.Log("RETUNING TO ORIGINAL POS!");
                healTimer = healTime;
                ReturnToOriginalPos();
            }
        }

        // HEALED
        if (returning && Mathf.Abs(transform.position.z - baseZ) < 0.05f)
        {
            isHurt = false;
            returning = false;
            hp = 2;
        }
    }

    private void HandlePlayerJump()
    {
        if (!isGrounded)
            return;
        
        playerSfxManager.PlayJumpSFX();

        // reset eventuale velocit� verticale residua
        Vector3 vel = rb.linearVelocity;
        vel.y = 0f;
        rb.linearVelocity = vel;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.y < 0)
        {
            // falls faster
            rb.AddForce(
                Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime,
                ForceMode.Acceleration
            );
        }
        else if (rb.linearVelocity.y > 0)
        {
            // salita leggermente smorzata (opzionale)
            rb.AddForce(
                Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1f),
                ForceMode.Acceleration
            );
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            hp--;
            if (hp <= 0)
            {
                OnPlayerDeath?.Invoke();
            }
            else
            {
                ApplyKnockback();
            }
        }
        if (collision.gameObject.CompareTag("OneshotObstacle"))
        {
            OnPlayerDeath?.Invoke();
        }
        
    }

    private void ApplyKnockback()
    {
        isHurt = true;
        returning = false;

        healTimer = healTime;
        targetZ = baseZ - knockbackDistance;
    }

    private void ReturnToOriginalPos()
    {
        returning = true;
        targetZ = baseZ;
    }

    private void HandlePlayerInput(Vector2 movementInput)
    {
        if (movementInput.x > 0.5f)
            ChangeLane(1);
        else if (movementInput.x < -0.5f)
            ChangeLane(-1);
    }

    private void ChangeLane(int direction) {
        int targetLane = Mathf.Clamp(currentLane + direction, 0, 2);
        // check if current lane is different, i.e. we are not in the borders
        if (targetLane == currentLane) return;

        currentLane = targetLane;
        targetPosition = new Vector3(
            (currentLane - 1) * laneOffset,
            transform.position.y,
            transform.position.z
        );
    }

    private void HandleEnterOnSquare()
    {
        this.enabled = false;
    }

    private void HandleExitOnSquare()
    {
        this.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawLine(originPoint + new Vector3(0,yOffset,0), originPoint + new Vector3(0, yOffset, 0)  + (Vector3.forward * 40));
        Handles.DrawLine(originPoint + new Vector3(0, yOffset, 0) - Vector3.left*5, originPoint + new Vector3(0, yOffset, 0) - Vector3.left * 5 + (Vector3.forward * 40));
        Handles.DrawLine(originPoint + new Vector3(0, yOffset, 0) + Vector3.left*5, originPoint + new Vector3(0, yOffset, 0) + Vector3.left * 5 + (Vector3.forward * 40));
    }

}
