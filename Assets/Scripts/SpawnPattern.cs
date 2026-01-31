using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Assets/Prefab/SpawnPatterns")]
public class SpawnPattern : MonoBehaviour
{
    [SerializeField] private List<GameObject> veryEasyPatterns = new();
    [SerializeField] private List<GameObject> easyPatterns = new();
    [SerializeField] private List<GameObject> MediumPatterns = new();
    [SerializeField] private List<GameObject> HardPatterns = new();




}
