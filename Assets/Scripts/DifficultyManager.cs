using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private int squareCounter;
    [Tooltip("X -> Squares, Y -> Difficulty Multiplier")]
    [SerializeField] private AnimationCurve difficultyCurve;
    public static float DifficultyMultiplier = 1f;

    private void Awake()
    {
        EntryPoint.OnPlayerEnterOnEntryPoint += IncreaseCounter;
    }

    private void OnDestroy()
    {
        EntryPoint.OnPlayerEnterOnEntryPoint -= IncreaseCounter;
    }

    private void IncreaseCounter()
    {
        squareCounter++;
        difficultyCurve.Evaluate(squareCounter);
    }
}
