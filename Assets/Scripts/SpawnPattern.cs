using UnityEngine;


[CreateAssetMenu(menuName = "Assets/Prefab/SpawnPatterns")]
public class SpawnPattern : ScriptableObject
{
    [System.Serializable]
    public class Pattern {
        public int rows = 5;
        public int cols = 3;

        [SerializeField]
        private SpawnDataMapping[] data = new SpawnDataMapping[15]; // 3 * 5

        public SpawnDataMapping Get(int row, int col)
        {
            return data[row * cols + col];
        }
    }

    [SerializeField] public Pattern pattern;

    public SpawnDataMapping GetSpawnPattern(int row, int col)
    {
        return pattern.Get(row, col);
    }
}
