using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class SpawnPattern : MonoBehaviour
{
    [SerializeField] private List<GameObject> veryEasyPatterns = new();
    [SerializeField] private List<GameObject> easyPatterns = new();
    [SerializeField] private List<GameObject> mediumPatterns = new();
    [SerializeField] private List<GameObject> hardPatterns = new();

    public GameObject GetRandomPattern(float difficulty)
    {
        // usiamo speedmultipier come difficoltà
        // es: 1.0 -> inizio, 3.0+ -> late game

        float veryEasyWeight = Mathf.Clamp01(2.5f - difficulty);
        float easyWeight = Mathf.Clamp01(3.0f - difficulty * 0.8f);
        float mediumWeight = Mathf.Clamp01(difficulty - 1.0f);
        float hardWeight = Mathf.Clamp01(difficulty - 2.0f);

        float total =
            veryEasyWeight +
            easyWeight +
            mediumWeight +
            hardWeight;

        float roll = Random.value * total;

        if (roll < veryEasyWeight)
            return Pick(veryEasyPatterns);

        roll -= veryEasyWeight;
        if (roll < easyWeight)
            return Pick(easyPatterns);

        roll -= easyWeight;
        if (roll < mediumWeight)
            return Pick(mediumPatterns);

        return Pick(hardPatterns);
    }

    private GameObject Pick(List<GameObject> list)
    {
        if (list.Count == 0) return null;
        return list[Random.Range(0, list.Count)];
    }


}
