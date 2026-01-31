using System;
using UnityEngine;

public class NextStagePoint : MonoBehaviour
{
    [SerializeField] private float radius = 2.5f;
    public Action OnPlayerEnterOnNextStagePoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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
