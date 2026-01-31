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


    private List<GameObject> squareStaticSegments = new();
    private List<GameObject> activeGroundSegments = new();
    private SpawnPattern currentSpawnPattern;
    private GameObject activeSquare;
    private int spawnPatternCounter = -1;
    private int counterSegmentsLeft = 100;
    private float squareEntryZ;
    private float squareStartTransform;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        currentSpawnPattern = patternPool.GetRandomPattern();
        activeGroundSegments.AddRange(groundSegments);
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

    void MoveWorld()
    {
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

        if (PlayerPassedSquareEntry())
        {
            SpawnEndSquareChunks();
            state = WorldState.InSquare;
            Debug.Log("In Square STATE");
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

        // piazza i segmenti in fondo
        float scale = 2 * activeGroundSegments[0].transform.lossyScale.x;

        // distanziali in base alla grandezza della piazza
        PopAndPushGround(groundSegments, 0, scale);
        // distanzia gli altri 3 in base alla lunghezza dei segmenti
        scale = groundSegments[0].transform.lossyScale.x;
        for (int i=0; i<3; i++)
        {
            PopAndPushGround(groundSegments, 0, scale);
        }
    }


    // creates square prefab and adds the condition to enter the new state
    void PrepareSquare()
    {
        GameObject lastSegment = groundSegments[^1];

        Vector3 spawnPos =
            lastSegment.transform.position +
            Vector3.forward * squarePrefab.transform.lossyScale.z;

        activeSquare = Instantiate(squarePrefab, spawnPos, Quaternion.identity);
        // add square to active segments for movement
        activeGroundSegments.Add(activeSquare);

        Debug.Log("Square prepared");
    }

    // checks if player passed the square entry point
    bool PlayerPassedSquareEntry()
    {
        squareStartTransform = activeSquare.transform.position.z - activeSquare.transform.lossyScale.z;
        Debug.Log("Player Z: " + transform.position.z + " Square Entry Z: " + squareStartTransform);
        return transform.position.z >= squareStartTransform;
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
    
    