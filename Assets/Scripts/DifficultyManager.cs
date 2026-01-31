using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private int squareCounter;
    [Tooltip("X -> Squares, Y -> Score Multiplier")]
    [SerializeField] private AnimationCurve scoreMultiplierCurve;

    private float speedMultiplier = 1f;
    private float speedFactor = .1f;

    public static float ScoreMultiplier = 1f;
    public static float SpeedMultiplier = 1f;

    private void Awake()
    {
        EntrySquarePoint.OnPlayerEnterOnEntryPoint += IncreaseCounter;
        GroundGenerationManager.OnSegmentCreation += IncreaseSpeedMultiplier;
    }

    private void OnDestroy()
    {
        EntrySquarePoint.OnPlayerEnterOnEntryPoint -= IncreaseCounter;
        GroundGenerationManager.OnSegmentCreation -= IncreaseSpeedMultiplier;
    }

    private void IncreaseCounter()
    {
        squareCounter++;
        scoreMultiplierCurve.Evaluate(squareCounter);
    }

    private void IncreaseSpeedMultiplier()
    {
        speedMultiplier += speedFactor;
        SpeedMultiplier = speedMultiplier;
    }
}
