using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class GroundGenerationManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> groundSegments_Middle = new();
    [SerializeField] private List<GameObject> groundSegments_Left = new();
    [SerializeField] private List<GameObject> groundSegments_Right = new();
    [SerializeField] private GameObject groundSegmentPrefab;
    [SerializeField] private PatternPool patternPool;
    [SerializeField] private float segmentLength = 20f;
    [SerializeField] private float offset = -30.0f;
    [SerializeField] private GameObject obstaclePrefab;

    private SpawnPattern currentSpawnPattern;
    private int spawnPatternCounter = -1;
    private List<GameObject> groundChunk = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
       currentSpawnPattern = patternPool.GetRandomPattern();
    }

    // Update is called once per frame
    void Update()
    {
        if (groundSegments_Middle[0].transform.position.z < offset)
        {
            AdvanceChunk();
        }
    }

    void AdvanceChunk()
    {
        spawnPatternCounter++;

        if (spawnPatternCounter >= 5)
        {
            spawnPatternCounter = 0;
            currentSpawnPattern = patternPool.GetRandomPattern();
        }

        PopAndPushGround(groundSegments_Left, offset, 0);
        PopAndPushGround(groundSegments_Middle, offset, 1);
        PopAndPushGround(groundSegments_Right, offset, 2);
    }


    void PopAndPushGround(List<GameObject> groundSegment, float offset, int column)
    {
        GameObject newSegment = groundSegment[0];
        float scale = newSegment.transform.lossyScale.x;
        groundSegment.RemoveAt(0);

        // clear anchors/obstacles
        Transform newSegmentAnchor = newSegment.transform.GetChild(0);
        for (int i = newSegmentAnchor.childCount - 1; i >= 0; i--)
        {
            Destroy(newSegmentAnchor.GetChild(i).gameObject);
        }

        GameObject lastSegment = groundSegment[^1];

        newSegment.transform.position = lastSegment.transform.position + new Vector3(0, 0, 1) * scale;

        // Determine if we need to spawn an object on this segment
        SpawnDataMapping isObject  = currentSpawnPattern.GetSpawnPattern(spawnPatternCounter, column);
            
        // spawn obstacle
        if (isObject.prefab != null)
        {
            GameObject anchor = newSegment.transform.GetChild(0).gameObject;
            GameObject obstacle = Instantiate(isObject.prefab, anchor.transform.position, Quaternion.identity);
            obstacle.transform.parent = anchor.transform;
            obstacle.transform.localPosition = isObject.originOffset;
        }
            
        groundSegment.Add(newSegment);
    }


}
    
    