using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SquareManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private GameObject actorPedestrianPrefab;
    [SerializeField] private GameObject otherPedestrianPrefab;
    [SerializeField] private GameObject policePedestrianPrefab;

    [SerializeField] private float pedestrianSpawnInterval = 0.5f;
    [SerializeField] private float numberOfPedestriansToSpawn = 2f;

    [SerializeField] private int maxPedestriansCanSpawnInSquare = 50;
    private int currentPedestrianCount = 0;

    [SerializeField] private float percentActorPedestrians = 0.5f;
    [SerializeField] private float percentOtherPedestrians = 0.4f;
    [SerializeField] private float percentPolicePedestrians = 0.1f;

    [SerializeField] private EntryPoint entryPoint;
    [SerializeField] private DepositPoint depositPoint;
    [SerializeField] private NextStagePoint nextStagePoint;

    private GameObject player;
    private PlayerController playerController;
    private PlayerController_Square playerController_Square;

    private void Awake()
    {
        // Find the player GameObject by Layer
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
            playerController_Square = player.GetComponent<PlayerController_Square>();
        }
    }

    private void OnEnable()
    {
        entryPoint.OnPlayerEnterOnEntryPoint += OnEntryPoint;
        depositPoint.OnPlayerEnterOnDepositPoint += OnDepositPoint;
        nextStagePoint.OnPlayerEnterOnNextStagePoint += OnNextStagePoint;
    }

    private void OnDisable()
    {
        entryPoint.OnPlayerEnterOnEntryPoint -= OnEntryPoint;
        depositPoint.OnPlayerEnterOnDepositPoint -= OnDepositPoint;
        nextStagePoint.OnPlayerEnterOnNextStagePoint -= OnNextStagePoint;
    }

    private IEnumerator SpawnPassants()
    {
        // Spawn Pedestrians
        for (int i = 0; i < numberOfPedestriansToSpawn; i++)
        {
            if (currentPedestrianCount >= maxPedestriansCanSpawnInSquare)
            {
                yield break;
            }

            SpawnPedestrian();
        }

        yield return new WaitForSeconds(pedestrianSpawnInterval);

        StartCoroutine(SpawnPassants());

    }

    private GameObject RandomPedestrian()
    {
        float randValue = Random.Range(0f, 1f);
        if (randValue < percentActorPedestrians)
        {
            return actorPedestrianPrefab;
        }
        else if (randValue < percentActorPedestrians + percentOtherPedestrians)
        {
            return otherPedestrianPrefab;
        }
        else
        {
            return policePedestrianPrefab;
        }
    }

    private Transform RandomSpawnPoint()
    {
        int randIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randIndex];
    }

    private Transform ReachTheMostDistantPoint(Transform fromPoint)
    {
        Transform mostDistantPoint = spawnPoints[0];
        float maxDistance = Vector3.Distance(fromPoint.position, mostDistantPoint.position);
        foreach (Transform point in spawnPoints)
        {
            float distance = Vector3.Distance(fromPoint.position, point.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                mostDistantPoint = point;
            }
        }
        return mostDistantPoint;
    }

    private void SpawnPedestrian()
    {
        GameObject pedestrianPrefab = RandomPedestrian();
        Transform spawnPoint = RandomSpawnPoint();
        Transform targetPoint = ReachTheMostDistantPoint(spawnPoint);
        GameObject pedestrian = Instantiate(pedestrianPrefab, spawnPoint.position, spawnPoint.rotation);
        Passant passant = pedestrian.GetComponent<Passant>();
        if (passant != null)
        {
            passant.SetTarget(targetPoint);
        }
        currentPedestrianCount++;
    }

    private void OnEntryPoint()
    {
        Debug.Log("Player entered the square. Initiating square...");
        entryPoint.gameObject.SetActive(false);
        StartCoroutine(SpawnPassants());
    }

    private void OnDepositPoint()
    {
        Debug.Log("Player entered the deposit point.");
    }
    
    private void OnNextStagePoint()
    {
        Debug.Log("Player entered the Next Stage Point");
    }
}
