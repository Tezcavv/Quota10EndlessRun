using UnityEngine;

public class HighScoreCounter : MonoBehaviour
{
    private int highScore = 0;
    [SerializeField]private int currentScoreTheatre = 0;
    [SerializeField]private int currentScoreCart = 0;
    //private int bestScore = 0;



    private void OnEnable()
    {
        highScore = PlayerPrefs.GetInt("HighScore" , 0);
    }

    public void AddScoreTheatre(float multiplier)
    {
        float currentScore= currentScoreCart * multiplier ;
        currentScoreTheatre += Mathf.RoundToInt(currentScore);

    }

    public void AddScoreCart(int score)
    {
        currentScoreCart += score;

    }

    public void SetHighScore()
    {
        if (currentScoreTheatre  > highScore)
        {
            highScore = currentScoreTheatre;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
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
        return currentScoreCart;
    }

    //public int GetBestScore()
    //{
    //    bestScore= PlayerPrefs.GetInt("BestScore");
    //    return bestScore;
    //}
}
