using System;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    public Action OnPlayerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke();
        }
    }
}
