using System;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private float radius = 2.5f;
    public Action OnPlayerEnterOnEntryPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPlayerEnterOnEntryPoint?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
