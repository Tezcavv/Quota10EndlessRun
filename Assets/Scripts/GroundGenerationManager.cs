using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.Rendering.HableCurve;


public class GroundGenerationManager : MonoBehaviour
{
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
    [SerializeField] private PatternPool patternPool;
    [SerializeField] private float squareEntryOffset = 5f;
    //[SerializeField] private float segmentLength = 20f;
    [FormerlySerializedAs("offset")] [SerializeField] private float destroyAtZ = -100.0f;
    //[SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float worldSpeed = 15f;

    [SerializeField] private PlayerSFXManager playerSfxManager;


    public List<GameObject> activeGroundSegments = new();
    public SpawnPattern currentSpawnPattern;
    public GameObject activeSquare;
    public int spawnPatternCounter = -1;
    public int counterSegmentsLeft = 1;
    public float lastWorldSpeed;
    private float destroySquare = 1f;
    private bool isSquareActive = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        currentSpawnPattern = patternPool.GetRandomPattern();
        activeGroundSegments.AddRange(groundSegments);
        EntrySquarePoint.OnPlayerEnterOnEntryPoint += OnSquareEnter;
        ExitSquarePoint.OnPlayerEnterOnExitPoint += OnSquareExit;
    }
        // Update is called once per frame
    void Update()
    {
        if (worldSpeed > 0f)
        {
            // playerSfxManager.PlayWalkSFX();
        }

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
        lastWorldSpeed = worldSpeed;
        worldSpeed = 0f;
        SpawnEndSquareChunks();
        state = WorldState.InSquare;
        Debug.Log("In Square STATE");
    }

    void OnSquareExit()
    {
        worldSpeed = lastWorldSpeed;
        counterSegmentsLeft = 5; // reset segments to run before next square
        state = WorldState.Running;
        activeGroundSegments.Clear();
        activeGroundSegments.AddRange(groundSegments);
        activeGroundSegments.Add(activeSquare);
        activeSquare = null;
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
                destroySquare = 1f;
            }
        }
        Debug.Log("Moving world at speed: " + worldSpeed);
        Vector3 delta = Vector3.back * worldSpeed * Time.deltaTime;

        foreach (GameObject seg in activeGroundSegments)
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

        Transform exitPoint = activeSquare.transform.Find("ExitSquarePoint");

        GameObject firstChunk = groundSegments[0];

        // rimuovilo temporaneamente dalla lista
        groundSegments.RemoveAt(0);

        // posizionamento preciso
        firstChunk.transform.position = exitPoint.position;

        // reinserisci in fondo
        groundSegments.Add(firstChunk);

        // distanzia gli altri 3 in base alla lunghezza dei segmenti
        float scale = 48f;
        for (int i=0; i<3; i++)
        {
            PopAndPushGround(groundSegments, 0, scale);
        }
    }


    // creates and positions square prefab
    void PrepareSquare()
    {
        GameObject lastSegment = groundSegments[^1];

        float lastChunkEndZ = lastSegment.transform.position.z + 48f;

        activeSquare = Instantiate(squarePrefab, Vector3.zero, Quaternion.identity);
        Transform startEntry = activeSquare.transform.Find("EntrySquarePoint");
        float offsetZ = activeSquare.transform.position.z - startEntry.position.z;

        Vector3 squarePos = new Vector3(
        lastSegment.transform.position.x,
        lastSegment.transform.position.y,
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
        if (spawnPatternCounter >= 5)
        {
            spawnPatternCounter = 0;
            currentSpawnPattern = patternPool.GetRandomPattern();
        }

        float scale = 48f; //todo apply to new models
        PopAndPushGround(groundSegments, 0, scale);
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
        SpawnDataMapping isObject  = currentSpawnPattern.GetSpawnPattern(spawnPatternCounter, column);
            
        // spawn obstacle
        if (isObject.prefab != null)
        {
            GameObject anchor = newSegment.transform.GetChild(0).gameObject;
            GameObject obstacle = Instantiate(isObject.prefab, anchor.transform.position, Quaternion.identity);
            obstacle.transform.parent = anchor.transform;
           // obstacle.transform.localPosition = isObject.originOffset;
        }
            
        //add to list, now count is 5 again
        groundSegment.Add(newSegment);
    }


}
    
    