using UnityEngine;


[CreateAssetMenu(menuName = "Assets/Prefab/SpawnPatterns")]
public class SpawnPattern : ScriptableObject
{
    public int rows = 5;
    public int cols = 3;

    [SerializeField]
    private int[] data = new int[15]; // 3 * 5

    public int Get(int row, int col)
    {
        return data[row * cols + col];
    }
}
