using UnityEngine;
using System.Collections.Generic;

public class PatternPool : MonoBehaviour
{
    [SerializeField] private List<SpawnPattern> patterns;

    public SpawnPattern GetRandomPattern()
    {
        return patterns[Random.Range(0, patterns.Count)];
    }
}
