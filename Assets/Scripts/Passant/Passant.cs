using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Passant : MonoBehaviour
{
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float anglularSpeed = 180f;

    [Header("Arrival")]
    [SerializeField] private float arrivalRadius = 2.5f;

    [SerializeField] private PassInfo passInfo;

    private Transform target;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"NavMeshAgent mancante su {name}");
            enabled = false;
            return;
        }

        agent.speed = Random.Range(minSpeed, maxSpeed);
        agent.angularSpeed = anglularSpeed;
        agent.updatePosition = true;
        agent.updateRotation = true;
    }

    private void Update()
    {
        if (target == null || !agent.isOnNavMesh)
            return;

        // Arrivo basato su raggio, non su stoppingDistance
        float sqrDistance = (transform.position - target.position).sqrMagnitude;

        if (sqrDistance <= arrivalRadius * arrivalRadius)
        {
            OnArrival();
        }
    }

    public void SetTarget(Transform targetPoint)
    {
        if (agent == null || !agent.isOnNavMesh || targetPoint == null)
            return;

        target = targetPoint;

        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    private void OnArrival()
    {
        agent.isStopped = true;
        Destroy(gameObject);
    }
}
