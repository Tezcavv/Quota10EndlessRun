using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.Rendering.HableCurve;


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
    [SerializeField] private List <GameObject> groundSegments_static = new();
    
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private SpawnPattern spawnPattern;
    [SerializeField] private float squareEntryOffset = 5f;
    [SerializeField] private GameObject playerPrefab;
    //[SerializeField] private float segmentLength = 20f;
    [FormerlySerializedAs("offset")] [SerializeField] private float destroyAtZ = -100.0f;
    //[SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float worldSpeed = 15f;

    public Action OnEnterEndlessMode;
    public Action OnEnterSquareMode;

    public List<GameObject> activeGroundSegments = new();
    public List<GameObject> activePatterns = new();
    public SpawnPattern currentSpawnPattern;
    public GameObject activeSquare;
    public int spawnPatternCounter = -1;
    public int counterSegmentsLeft = 10;
    public float lastWorldSpeed;
    private float destroySquare = 5f;
    private bool isSquareActive = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        activeGroundSegments.AddRange(groundSegments);
        EntrySquarePoint.OnPlayerEnterOnEntryPoint += OnSquareEnter;
        ExitSquarePoint.OnPlayerEnterOnExitPoint += OnSquareExit;
    }
        // Update is called once per frame
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
                UpdateSquare();
                break;
        }

    }

    void OnSquareEnter()
    {
        OnEnterSquareMode?.Invoke();
        lastWorldSpeed = worldSpeed;
        worldSpeed = 0f;
        foreach(GameObject seg in groundSegments_static)
        {
            activeGroundSegments.Remove(seg);
        }
        state = WorldState.InSquare;
        Debug.Log("In Square STATE");
    }

    void OnSquareExit()
    {
        OnEnterEndlessMode?.Invoke();
        worldSpeed = lastWorldSpeed;
        counterSegmentsLeft = 5; // reset segments to run before next square
        state = WorldState.Running;
        destroyAtZ = activeSquare.GetComponentInChildren<ExitSquarePoint>().transform.position.z - 60;
        activeGroundSegments.Clear();
        activeGroundSegments.AddRange(groundSegments);
        activeGroundSegments.Add(activeSquare);
        activeSquare = null;
        playerPrefab.transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        Debug.Log("Square Exited: resetting to RUNNING state");
    }

    void MoveWorld()
    {
        if (isSquareActive)
        {
            destroySquare -= Time.deltaTime;
            if (destroySquare <= 0f)
            {
                Destroy(activeSquare);
                isSquareActive = false;
                destroySquare = 5f;
            }
        }
        //Debug.Log("Moving world at speed: " + worldSpeed);
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

    // running state, subway surfer shit
    void UpdateRunning()
    {
        if (groundSegments[0].transform.position.z < destroyAtZ)
        {
            AdvanceChunk();
        }
        if (activePatterns.Count > 0)
        {
            if (activePatterns[0].transform.position.z < destroyAtZ)
            {
                Destroy(activePatterns[0]);
                activePatterns.RemoveAt(0);
            }
        }

        if (counterSegmentsLeft <= 0)
        {
            state = WorldState.PreparingSquare;
            Debug.Log("Preparing Square STATE");
        }
    }
    
    // preparing the square switch state
    void UpdatePreparingSquare()
    {
        if (activeSquare == null)
        {
            PrepareSquare();
            SpawnEndSquareChunks();
        }
    }

    void AlignStaticToDynamic(List<GameObject> dynamicList, List<GameObject> staticList)
    {
        int count = Mathf.Min(dynamicList.Count, staticList.Count);

        for (int i = 0; i < count; i++)
        {
            staticList[i].transform.position =
                dynamicList[i].transform.position;
        }
    }


    void SpawnEndSquareChunks()
    {
        AlignStaticToDynamic(groundSegments, groundSegments_static);

        (groundSegments, groundSegments_static) =
            (groundSegments_static, groundSegments);

        Transform exitPoint = activeSquare.GetComponentInChildren<ExitSquarePoint>().transform;

        GameObject firstChunk = groundSegments[0];

        // rimuovilo temporaneamente dalla lista
        groundSegments.RemoveAt(0);

        // posizionamento preciso
        firstChunk.transform.position = exitPoint.position + new Vector3(0,0,1f);

        // reinserisci in fondo
        groundSegments.Add(firstChunk);

        // distanzia gli altri 3 in base alla lunghezza dei segmenti
        float scale = 48f;
        for (int i=0; i<3; i++)
        {
            PopAndPushGround(groundSegments, 0, scale);
        }
        activeGroundSegments.AddRange(groundSegments);
    }


    // creates and positions square prefab
    void PrepareSquare()
    {
        GameObject lastSegment = groundSegments[^1];

        float lastChunkEndZ = lastSegment.transform.position.z + 50f;

        activeSquare = Instantiate(squarePrefab, Vector3.zero, Quaternion.identity);
        Transform startEntry = activeSquare.transform.Find("EntryPoint");
        float offsetZ = activeSquare.transform.position.z - startEntry.position.z;

        Vector3 squarePos = new Vector3(
        lastSegment.transform.position.x,
        lastSegment.transform.position.y - 0.4f,
        lastChunkEndZ + offsetZ
        );
        activeSquare.transform.position = squarePos;

        // add square to active segments for movement
        activeGroundSegments.Add(activeSquare);

        Debug.Log("Square prepared");
    }


    void AdvanceChunk()
    {
        spawnPatternCounter++;
        counterSegmentsLeft--;
        float scale = 48f; //todo apply to new models
        OnSegmentCreation?.Invoke();
        PopAndPushGround(groundSegments, 0, scale);
        GameObject pattern = currentSpawnPattern.GetRandomPattern(DifficultyManager.SpeedMultiplier);
        GameObject g=Instantiate(pattern, groundSegments[^1].transform.position, Quaternion.identity);
        activePatterns.Add(g);
    }


    void UpdateSquare()
    {
        worldSpeed = 0f;
        // BLA BLA BLA
    }

    void PopAndPushGround(List<GameObject> groundSegment, int column, float scale)
    {
        GameObject newSegment = groundSegment[0];
        
        groundSegment.RemoveAt(0); //rimuovo quello dietro

        // clear anchors/obstacles
        //Transform newSegmentAnchor = newSegment.transform.GetChild(0);
        //for (int i = newSegmentAnchor.childCount - 1; i >= 0; i--)
        //{
        //    Destroy(newSegmentAnchor.GetChild(i).gameObject); //distrugge tutti gli ostacoli
        //}

        //list has count  4 
        GameObject lastSegment = groundSegment[^1];

        newSegment.transform.position = lastSegment.transform.position + new Vector3(0, 0, 1) * scale; //todo apply proper offset

        // Determine if we need to spawn an object on this segment
        //SpawnDataMapping isObject  = currentSpawnPattern.GetSpawnPattern(spawnPatternCounter, column);
            
        // spawn obstacle
        //if (isObject.prefab != null)
        //{
        //    GameObject anchor = newSegment.transform.GetChild(0).gameObject;
        //    GameObject obstacle = Instantiate(isObject.prefab, anchor.transform.position, Quaternion.identity);
        //    obstacle.transform.parent = anchor.transform;
        //   // obstacle.transform.localPosition = isObject.originOffset;
        //}
            
        //add to list, now count is 5 again
        groundSegment.Add(newSegment);
    }


}
    
    