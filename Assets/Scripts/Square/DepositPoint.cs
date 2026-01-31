using System;
using UnityEngine;

public class DepositPoint : MonoBehaviour
{
    [SerializeField] private float radius = 2.5f;
    public static Action OnPlayerEnterOnDepositPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlayerEnterOnDepositPoint?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
