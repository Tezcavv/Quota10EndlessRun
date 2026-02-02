using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GroundGenerationManager : MonoBehaviour
{
    public static event Action OnSegmentCreation;

    private enum WorldState
    {
        Running,
        PreparingSquare,
        InSquare
    }

    private WorldState state = WorldState.Running;

    [SerializeField] private List<GameObject> groundSegments = new();
    [SerializeField] private List<GameObject> groundSegments_static = new();

    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private SpawnPattern spawnPattern;
    [SerializeField] private GameObject playerPrefab;
    [FormerlySerializedAs("offset")][SerializeField] private float destroyAtZ = -100f;
    [SerializeField] private float worldSpeed = 15f;

    public Action OnEnterEndlessMode;
    public Action OnEnterSquareMode;

    public List<GameObject> activeGroundSegments = new();
    public List<GameObject> activePatterns = new();

    private GameObject activeSquare;

    public int spawnPatternCounter = -1;
    public int counterSegmentsLeft = 10;
    public int originalCounterSegments = 10;

    private float lastWorldSpeed;
    private float originalWorldSpeed;
    private float destroySquareDelay = 2f;

    private void Awake()
    {
        activeGroundSegments.AddRange(groundSegments);

        EntrySquarePoint.OnPlayerEnterOnEntryPoint += OnSquareEnter;
        ExitSquarePoint.OnPlayerEnterOnExitPoint += OnSquareExit;

        originalWorldSpeed = worldSpeed;
    }

    private void Update()
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

    // =========================
    // SQUARE ENTER
    // =========================
    private void OnSquareEnter()
    {
        if (state != WorldState.PreparingSquare)
            return;

        state = WorldState.InSquare;

        OnEnterSquareMode?.Invoke();

        lastWorldSpeed = worldSpeed;
        worldSpeed = 0f;

        foreach (var seg in groundSegments_static)
            activeGroundSegments.Remove(seg);
    }

    // =========================
    // SQUARE EXIT
    // =========================
    private void OnSquareExit()
    {
        if (state != WorldState.InSquare)
            return;

        if (activeSquare == null)
        {
            Debug.LogError("Square exit called but activeSquare is null");
            return;
        }

        // 1. Esci dallo stato Square
        state = WorldState.Running;

        OnEnterEndlessMode?.Invoke();

        // 2. Ripristina world
        worldSpeed = lastWorldSpeed;
        counterSegmentsLeft = originalCounterSegments;

        // 3. Usa activeSquare PRIMA di distruggerla
        destroyAtZ = activeSquare.transform.position.z - 60f;

        activeGroundSegments.Clear();
        activeGroundSegments.AddRange(groundSegments);
        activeGroundSegments.Add(activeSquare);

        // 4. Distruzione ritardata
        StartCoroutine(DestroySquareRoutine(activeSquare));

        // 5. Ora è sicuro azzerare
        activeSquare = null;

        playerPrefab.transform.rotation =
            Quaternion.LookRotation(Vector3.forward, Vector3.up);
    }

    // =========================
    // CLEANUP (NO LOGICA)
    // =========================
    private IEnumerator DestroySquareRoutine(GameObject square)
    {
        yield return new WaitForSeconds(destroySquareDelay);

        if (square != null)
        {
            activeGroundSegments.Remove(square);
            Destroy(square);
        }
    }

    // =========================
    // WORLD MOVE
    // =========================
    private void MoveWorld()
    {
        Vector3 delta = Vector3.back * worldSpeed * Time.deltaTime;

        foreach (var seg in activeGroundSegments)
            seg.transform.position += delta;

        foreach (var p in activePatterns)
            p.transform.position += delta;
    }

    // =========================
    // RUNNING
    // =========================
    private void UpdateRunning()
    {
        if (groundSegments[0].transform.position.z < destroyAtZ)
            AdvanceChunk();

        if (activePatterns.Count > 0 &&
            activePatterns[0].transform.position.z < destroyAtZ)
        {
            Destroy(activePatterns[0]);
            activePatterns.RemoveAt(0);
        }

        if (counterSegmentsLeft <= 0)
            state = WorldState.PreparingSquare;
    }

    // =========================
    // PREPARING SQUARE
    // =========================
    private void UpdatePreparingSquare()
    {
        if (activeSquare != null)
            return;

        PrepareSquare();
        SpawnEndSquareChunks();
    }

    // =========================
    // PREPARE SQUARE
    // =========================
    private void PrepareSquare()
    {
        GameObject lastSegment = groundSegments[^1];
        float lastChunkEndZ = lastSegment.transform.position.z + 50f;

        activeSquare = Instantiate(squarePrefab);
        Transform entry = activeSquare.transform.Find("EntryPoint");

        float offsetZ = activeSquare.transform.position.z - entry.position.z;

        activeSquare.transform.position = new Vector3(
            lastSegment.transform.position.x,
            lastSegment.transform.position.y - 0.4f,
            lastChunkEndZ + offsetZ
        );

        activeGroundSegments.Add(activeSquare);
    }

    // =========================
    // END SQUARE CHUNKS
    // =========================
    private void SpawnEndSquareChunks()
    {
        AlignStaticToDynamic(groundSegments, groundSegments_static);

        int graphic = Random.Range(0, Enum.GetValues(typeof(GraphicType)).Length);
        foreach (var seg in groundSegments_static)
            seg.GetComponent<EndlessSegment>()?.ActivateObjects((GraphicType)graphic);

        (groundSegments, groundSegments_static) =
            (groundSegments_static, groundSegments);

        Transform exit = activeSquare.GetComponentInChildren<ExitSquarePoint>().transform;

        GameObject first = groundSegments[0];
        groundSegments.RemoveAt(0);

        first.transform.position = exit.position + Vector3.forward;
        groundSegments.Add(first);

        float scale = 48f;
        for (int i = 0; i < 3; i++)
            PopAndPushGround(groundSegments, scale);

        activeGroundSegments.AddRange(groundSegments);
    }

    // =========================
    // CHUNKS
    // =========================
    private void AdvanceChunk()
    {
        spawnPatternCounter++;
        counterSegmentsLeft--;

        OnSegmentCreation?.Invoke();

        float scale = 48f;
        PopAndPushGround(groundSegments, scale);

        worldSpeed = Mathf.Min(
            originalWorldSpeed + DifficultyManager.SpeedMultiplier * 3f, 60f);

        GameObject pattern =
            spawnPattern.GetRandomPattern(DifficultyManager.SpeedMultiplier / 2f);

        activePatterns.Add(
            Instantiate(pattern, groundSegments[^1].transform.position, Quaternion.identity));
    }

    private void PopAndPushGround(List<GameObject> segments, float scale)
    {
        GameObject seg = segments[0];
        segments.RemoveAt(0);

        GameObject last = segments[^1];
        seg.transform.position = last.transform.position + Vector3.forward * scale;

        segments.Add(seg);
    }

    private void AlignStaticToDynamic(List<GameObject> dynamic, List<GameObject> statics)
    {
        int count = Mathf.Min(dynamic.Count, statics.Count);
        for (int i = 0; i < count; i++)
            statics[i].transform.position = dynamic[i].transform.position;
    }
}
