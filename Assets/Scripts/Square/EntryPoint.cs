using System;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private float radius = 2.5f;
    public static Action OnPlayerEnterOnEntryPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlayerEnterOnEntryPoint?.Invoke();
            this.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
