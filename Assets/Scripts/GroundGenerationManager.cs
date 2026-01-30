using UnityEngine;
using System.Collections.Generic;

public class GroundGenerationManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> groundSegments_Middle = new();
    [SerializeField] private List<GameObject> groundSegments_Left = new();
    [SerializeField] private List<GameObject> groundSegments_Right = new();
    [SerializeField] private GameObject groundSegmentPrefab;
    [SerializeField] private float segmentLength = 10f;
    [SerializeField] private float offset = -30.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PopAndPushGround(groundSegments_Middle, offset);
        PopAndPushGround(groundSegments_Left, offset);
        PopAndPushGround(groundSegments_Right, offset);
    }

    void PopAndPushGround(List<GameObject> groundSegment, float offset)
    {
        if (groundSegment[0].transform.position.z < offset)
        {
            GameObject first = groundSegment[0];
            groundSegment.RemoveAt(0);

            GameObject last = groundSegment[^1];

            first.transform.position = last.transform.position + new Vector3(0, 0, 1) * segmentLength;

            groundSegment.Add(first);
        }
    }


}
    
    