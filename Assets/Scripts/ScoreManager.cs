using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event System.Action<int> OnScoreChanged = delegate { };
    private int highScore = 0;
    [SerializeField]private int currentScoreTheatre = 0;
    [SerializeField]private int currentScoreCart = 0;
    //private int bestScore = 0;
    private int Score;

    public int CurrentScoreCart 
    { get => currentScoreCart;
        set 
        { 
            if (currentScoreCart != value)
            {
                OnScoreChanged?.Invoke(currentScoreCart);
            }
            currentScoreCart = value;
        }
    }

    private void OnEnable()
    {
        highScore = PlayerPrefs.GetInt("HighScore" , 0);
    }

    private void Awake()
    {
        CartManager.OnCartCollided += addPassant; 
        DepositTheatrePoint.OnPlayerEnterOnDepositPoint += AddScoreTheatre;
    }
    private void OnDestroy()
    {
        CartManager.OnCartCollided -= addPassant;
        DepositTheatrePoint.OnPlayerEnterOnDepositPoint -= AddScoreTheatre;
    }

    public void AddScoreTheatre()
    {
        float currentScore = CurrentScoreCart * DifficultyManager.DifficultyMultiplier;
        currentScoreTheatre += Mathf.RoundToInt(currentScore);
        CurrentScoreCart = 0;

        if (currentScoreTheatre < 0)
        {
            currentScoreTheatre = 0;
        }

    }

    public void AddScoreCart(int score)
    {
        CurrentScoreCart += score;

        if (CurrentScoreCart < 0)
        {
            CurrentScoreCart = 0;
        }

    }

    public void SetHighScore()
    {
        if (currentScoreTheatre  > highScore)
        {
            highScore = currentScoreTheatre;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

    }

    private void addPassant(Passant passant)
    {
       Score = passant.PassInfo.hitScore;
       AddScoreCart(Score);
    }

    //public void SetBestScore()
    //{
    //    if (currentScoreTheatre+currentScoreCart > bestScore)
    //    {
    //        bestScore = currentScoreTheatre+currentScoreCart;
    //        PlayerPrefs.SetInt("BestScore", bestScore);
    //    }
    //}


    public int GetHighScore()
    {
        highScore= PlayerPrefs.GetInt("HighScore",0);
        return highScore;
    }

    public int GetCurrentScoreTheatre()
    {
        return currentScoreTheatre;
    }

    public int GetCurrentScoreCart()
    {
        return CurrentScoreCart;
    }

    //public int GetBestScore()
    //{
    //    bestScore= PlayerPrefs.GetInt("BestScore");
    //    return bestScore;
    //}
}
