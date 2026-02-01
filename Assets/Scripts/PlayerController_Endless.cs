using System;
using System.Collections;
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
    [SerializeField] private float invincibilityDuration = 2f;
    [SerializeField] private PlayerSFXManager playerSfxManager;
    [SerializeField] private Color hurtColor = Color.red;
    [SerializeField] private float blinkInterval = 0.1f;

    public Rigidbody rb;
    public bool isGrounded;
    public int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    public Vector3 targetPosition;
    public Vector3 originPoint;
    public int hp = 2; // va pi� lento ad un hp, ma pu� recuperarlo.
    public bool isHurt = false;
    public float healTimer = 0f;
    public float baseZ;
    public float targetZ;
    public bool returning = false;
    public bool isInvincible = false;
    public float invincibilityTimer = 0f;
    public static bool isDead = false;
    private Coroutine hurtCoroutine;
    private Coroutine invincibilityCoroutine;
    private Renderer[] renderers;
    private Color[] originalColors;
    private Coroutine blinkCoroutine;

    public Action OnPlayerDeath;
    public Action OnPlayerHurt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        isDead = false;
        rb = GetComponent<Rigidbody>();

        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].material.color;
        }

        InputManager.OnPlayerMovement += HandlePlayerInput;
        InputManager.OnPlayerJump += HandlePlayerJump;

        EntrySquarePoint.OnPlayerEnterOnEntryPoint += HandleEnterOnSquare;

        ExitSquarePoint.OnPlayerEnterOnExitPoint += HandleExitOnSquare;

        targetPosition = transform.position;
        originPoint = transform.forward;

        baseZ = transform.position.z;
        targetZ = baseZ;
        invincibilityTimer = invincibilityDuration;
        healTimer = healTime;
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
        //if (isHurt)
        //{
        //    checkInvincibility();
        //    playerSfxManager.PlayHurtSFX();

        //    float speed = returning ? returnSpeed : knockbackSpeed;

        //    Vector3 curr_pos = transform.position;
        //    curr_pos.z = Mathf.Lerp(curr_pos.z, targetZ, speed * Time.deltaTime);
        //    transform.position = curr_pos;

        //    healTimer -= Time.deltaTime;

        //    if (healTimer <= 0f)
        //    {
        //        Debug.Log("RETUNING TO ORIGINAL POS!");
        //        healTimer = healTime;
        //        //ReturnToOriginalPos();
        //        isHurt = false;
        //    }
        //}
    }

    private void checkInvincibility() 
    {
        invincibilityTimer -= Time.deltaTime;
        if (invincibilityTimer <= 0f)
        {
            isInvincible = false;
            invincibilityTimer = invincibilityDuration;
        }
    }
    private void HandlePlayerJump()
    {

        if (isDead) return;

        if (!this.enabled)
            return;
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

        if (isDead) return;
        
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
        if (isDead)
            return;

        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (isInvincible) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player hurt");
            hp--;
            OnPlayerHurt?.Invoke();

            if (hp <= 0)
            {
                isDead = true;
                OnPlayerDeath?.Invoke();
                
            }
            else
            {
                hurtCoroutine = StartCoroutine(HurtRoutine());
                invincibilityCoroutine = StartCoroutine(InvincibilityRoutine());
                blinkCoroutine = StartCoroutine(BlinkInvincibilityRoutine());
            }
        }
        if (collision.gameObject.CompareTag("OneshotObstacle"))
        {
            isDead = true;
            OnPlayerHurt?.Invoke();
            OnPlayerDeath?.Invoke();
        }
        

    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
    }

    private IEnumerator HurtRoutine()
    {
        isHurt = true;

        yield return new WaitForSeconds(healTime);

        isHurt = false;
        hp += 1;
    }

    private IEnumerator BlinkInvincibilityRoutine()
    {
        bool isRed = false;

        while (isInvincible)
        {
            isRed = !isRed;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = isRed ? hurtColor : originalColors[i];
            }

            yield return new WaitForSeconds(blinkInterval);
        }

        // Ripristino finale
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = originalColors[i];
        }

        blinkCoroutine = null;
    }



    //private void ApplyKnockback()
    //{
    //    isHurt = true;
    //    returning = false;

    //    healTimer = healTime;
    //    targetZ = baseZ - knockbackDistance;
    //}

    //private void ReturnToOriginalPos()
    //{
    //    returning = true;
    //    targetZ = baseZ;
    //}

    private void HandlePlayerInput(Vector2 movementInput)
    {
        if (isDead) return;
        if(!this.enabled)
            return;
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



}
