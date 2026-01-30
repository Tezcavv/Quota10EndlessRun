using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float laneOffset = 2.5f;
    [SerializeField] private float laneChangeSpeed = 10f;
    [SerializeField] private float yOffset = 5f;
    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    private Vector3 targetPosition;
    private Vector3 originPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        InputManager.OnPlayerMovement += HandlePlayerInput;
        InputManager.OnPlayerJump += HandlePlayerJump;
        targetPosition = transform.position;
        originPoint = transform.forward;
    }

    void OnDestroy()
    {
        InputManager.OnPlayerMovement -= HandlePlayerInput;
        InputManager.OnPlayerJump -= HandlePlayerJump;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            laneChangeSpeed * Time.deltaTime
        );
    }

    private void HandlePlayerJump()
    {
        
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

    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawLine(originPoint + new Vector3(0,yOffset,0), originPoint + new Vector3(0, yOffset, 0)  + (Vector3.forward * 40));
        Handles.DrawLine(originPoint + new Vector3(0, yOffset, 0) - Vector3.left*5, originPoint + new Vector3(0, yOffset, 0) - Vector3.left * 5 + (Vector3.forward * 40));
        Handles.DrawLine(originPoint + new Vector3(0, yOffset, 0) + Vector3.left*5, originPoint + new Vector3(0, yOffset, 0) + Vector3.left * 5 + (Vector3.forward * 40));
    }

}
