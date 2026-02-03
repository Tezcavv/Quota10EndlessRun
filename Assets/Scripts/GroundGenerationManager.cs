using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GroundGenerationManager : MonoBehaviour
{
    public static event System.Action OnSegmentCreation;

    // WORLD STATE MACHINE
    private enum WorldState
    {
        Running,
        PreparingSquare,
        InSquare
    }

    private WorldState state = WorldState.Running;

    [SerializeField] private List<GameObject> groundSegments = new();

    // questi vengono usati quando si cambia alla square mode
    [SerializeField] private List<GameObject> groundSegments_static = new();

    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private SpawnPattern spawnPattern;
    [SerializeField] private float squareEntryOffset = 5f;
    [SerializeField] private GameObject playerPrefab;
    [FormerlySerializedAs("offset")][SerializeField] private float destroyAtZ = -100.0f;
    [SerializeField] private float worldSpeed = 15f;

    public Action OnEnterEndlessMode;
    public Action OnEnterSquareMode;

    public List<GameObject> activeGroundSegments = new();
    public List<GameObject> activePatterns = new();
    public GameObject activeSquare;
    public int spawnPatternCounter = -1;
    public int counterSegmentsLeft = 10;
    public float lastWorldSpeed;
    public float originalWorldSpeed;
    private float destroySquare = 2f;

    // usata come guardia di stato reale
    private bool isSquareActive = false;

    public bool triggered = false;
    public Coroutine destroySquareCoroutine;
    public int originalCounterSegments = 10;

    private void Awake()
    {
        activeGroundSegments.AddRange(groundSegments);
        EntrySquarePoint.OnPlayerEnterOnEntryPoint += OnSquareEnter;
        ExitSquarePoint.OnPlayerEnterOnExitPoint += OnSquareExit;
        originalWorldSpeed = worldSpeed;
    }

    void Update()
    {
        switch (state)
        {
            case WorldState.Running:
                MoveWorld();
                UpdateRunning();
                break;

            case WorldState.PreparingSquare:
                MoveWorld();
                UpdatePreparingSquare();
                break;

            case WorldState.InSquare:
                worldSpeed = 0f;
                break;
        }
    }

    void OnSquareEnter()
    {
        OnEnterSquareMode?.Invoke();
        lastWorldSpeed = worldSpeed;
        worldSpeed = 0f;

        foreach (GameObject seg in groundSegments_static)
        {
            activeGroundSegments.Remove(seg);
        }

        state = WorldState.InSquare;
        Debug.Log("In Square STATE");
    }

    void OnSquareExit()
    {
        // evita chiamate multiple mentre la square è in teardown
        if (triggered || activeSquare == null)
            return;

        OnEnterEndlessMode?.Invoke();
        worldSpeed = lastWorldSpeed;
        counterSegmentsLeft = originalCounterSegments;
        state = WorldState.Running;

        // activeSquare viene usata solo se valida
        destroyAtZ = activeSquare.transform.position.z - 60;

        activeGroundSegments.Clear();
        activeGroundSegments.AddRange(groundSegments);
        activeGroundSegments.Add(activeSquare);

        triggered = true;
        destroySquareCoroutine = StartCoroutine(DestroySquareRoutine());

        playerPrefab.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        Debug.Log("Square Exited: resetting to RUNNING state");
    }

    private IEnumerator DestroySquareRoutine()
    {
        yield return new WaitForSeconds(destroySquare);

        if (activeSquare != null)
        {
            activeGroundSegments.Remove(activeSquare);
            Destroy(activeSquare);
            activeSquare = null;
        }

        // reset controllato dello stato
        isSquareActive = false;
        triggered = false;
    }

    void MoveWorld()
    {
        Vector3 delta = Vector3.back * worldSpeed * Time.deltaTime;

        foreach (GameObject seg in activeGroundSegments)
        {
            seg.transform.position += delta;
        }

        foreach (GameObject seg in activePatterns)
        {
            seg.transform.position += delta;
        }
    }

    void UpdateRunning()
    {
        if (groundSegments[0].transform.position.z < destroyAtZ)
        {
            AdvanceChunk();
        }

        if (activePatterns.Count > 0 &&
            activePatterns[0].transform.position.z < destroyAtZ)
        {
            Destroy(activePatterns[0]);
            activePatterns.RemoveAt(0);
        }

        if (counterSegmentsLeft <= 0)
        {
            originalCounterSegments += originalCounterSegments;
            state = WorldState.PreparingSquare;
            Debug.Log("Preparing Square STATE");
        }
    }

    void UpdatePreparingSquare()
    {
        // la square viene preparata UNA SOLA VOLTA
        if (!isSquareActive)
        {
            isSquareActive = true;
            PrepareSquare();
            SpawnEndSquareChunks();
        }
    }

    void AlignStaticToDynamic(List<GameObject> dynamicList, List<GameObject> staticList)
    {
        int count = Mathf.Min(dynamicList.Count, staticList.Count);

        for (int i = 0; i < count; i++)
        {
            staticList[i].transform.position = dynamicList[i].transform.position;
        }
    }

    void SpawnEndSquareChunks()
    {
        AlignStaticToDynamic(groundSegments, groundSegments_static);

        int newGraphicType = Random.Range(0, Enum.GetValues(typeof(GraphicType)).Length);
        foreach (GameObject seg in groundSegments_static)
        {
            seg.TryGetComponent(out EndlessSegment endlessSegment);
            endlessSegment?.ActivateObjects((GraphicType)newGraphicType);
        }

        (groundSegments, groundSegments_static) =
            (groundSegments_static, groundSegments);

        Transform exitPoint = activeSquare.GetComponentInChildren<ExitSquarePoint>().transform;

        GameObject firstChunk = groundSegments[0];
        groundSegments.RemoveAt(0);
        firstChunk.transform.position = exitPoint.position + new Vector3(0, 0, 1f);
        groundSegments.Add(firstChunk);

        float scale = 48f;
        for (int i = 0; i < 3; i++)
        {
            PopAndPushGround(groundSegments, 0, scale);
        }

        activeGroundSegments.AddRange(groundSegments);
    }

    void PrepareSquare()
    {
        GameObject lastSegment = groundSegments[^1];
        float lastChunkEndZ = lastSegment.transform.position.z + 50f;

        activeSquare = Instantiate(squarePrefab, Vector3.zero, Quaternion.identity);
        Transform startEntry = activeSquare.transform.Find("EntryPoint");
        float offsetZ = activeSquare.transform.position.z - startEntry.position.z;

        activeSquare.transform.position = new Vector3(
            lastSegment.transform.position.x,
            lastSegment.transform.position.y - 0.4f,
            lastChunkEndZ + offsetZ
        );

        activeGroundSegments.Add(activeSquare);
        Debug.Log("Square prepared");
    }

    void AdvanceChunk()
    {
        spawnPatternCounter++;
        counterSegmentsLeft--;

        float scale = 48f;
        OnSegmentCreation?.Invoke();
        PopAndPushGround(groundSegments, 0, scale);

        worldSpeed = Mathf.Min(
            originalWorldSpeed + (DifficultyManager.SpeedMultiplier) * 3,
            60
        );

        GameObject pattern =
            spawnPattern.GetRandomPattern(DifficultyManager.SpeedMultiplier / 2);

        GameObject g =
            Instantiate(pattern, groundSegments[^1].transform.position, Quaternion.identity);

        activePatterns.Add(g);
    }

    void PopAndPushGround(List<GameObject> groundSegment, int column, float scale)
    {
        GameObject newSegment = groundSegment[0];
        groundSegment.RemoveAt(0);

        GameObject lastSegment = groundSegment[^1];
        newSegment.transform.position =
            lastSegment.transform.position + Vector3.forward * scale;

        groundSegment.Add(newSegment);
    }
}
