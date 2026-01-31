using System;
using UnityEngine;

public class NextStagePoint : MonoBehaviour
{
    [SerializeField] private float radius = 2.5f;
    public static Action OnPlayerEnterOnNextStagePoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlayerEnterOnNextStagePoint?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
