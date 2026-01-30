using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnPlayerMovement = delegate { };
    public void OnMove(InputValue value)
    {
        Vector2 inputValue = value.Get<Vector2>();
        Debug.Log(inputValue);
        OnPlayerMovement?.Invoke(inputValue);
    }
}
