using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnPlayerMovement = delegate { };
    public void OnMove(InputValue value)
    {

        if (PauseManager.IsPaused) return;
        
        Vector2 inputValue = value.Get<Vector2>();
        Debug.Log(inputValue);
        OnPlayerMovement?.Invoke(inputValue);
    }

    public static event Action OnPlayerJump = delegate { };
    public void OnJump()
    {
        if (PauseManager.IsPaused) return;
        
        Debug.Log("Jump pressed");
        OnPlayerJump?.Invoke();
    }

}
