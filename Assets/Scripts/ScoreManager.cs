using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static event System.Action<int> OnCartScoreChanged = delegate { };
    public static event System.Action<int> OnTheaterScoreChanged = delegate { };

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
                OnCartScoreChanged?.Invoke(currentScoreCart);
            }
            currentScoreCart = value;
        }
    }

    public int CurrentScoreTheatre
    {
        get => currentScoreTheatre;
        set
        {
            currentScoreTheatre = value;
            if(currentScoreTheatre != value)
                OnTheaterScoreChanged?.Invoke(currentScoreTheatre);
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
        float currentScore = CurrentScoreCart * DifficultyManager.ScoreMultiplier;
        CurrentScoreTheatre += Mathf.RoundToInt(currentScore);
        CurrentScoreCart = 0;

        if(CurrentScoreTheatre > highScore)
        {
            highScore = CurrentScoreTheatre;
        }

        if (CurrentScoreTheatre < 0)
        {
            CurrentScoreTheatre = 0;
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
        if (CurrentScoreTheatre  > highScore)
        {
            highScore = CurrentScoreTheatre;
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
        return CurrentScoreTheatre;
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
