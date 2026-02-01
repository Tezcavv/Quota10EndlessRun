
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score_UI : MonoBehaviour
{
    [SerializeField]Canvas scoreCanvas;
    [SerializeField] TextMeshProUGUI scoreTextCart, scoreTextTheatre;
    [SerializeField]ScoreManager highScoreCounter;

    public PlayerController_Endless player;
    
    public GameObject[] hearts = new GameObject[2];
    void Start()
    {
        scoreCanvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreTextCart.text = "Cart Score: " + highScoreCounter.GetCurrentScoreCart().ToString();
        scoreTextTheatre.text = "Theatre Score: " + highScoreCounter.GetCurrentScoreTheatre().ToString();
        hearts[0].SetActive(player.hp > 0);
        hearts[1].SetActive(player.hp > 1);

        if (PlayerController_Endless.isDead)
        {
            hearts[0].SetActive(false);
            hearts[1].SetActive(false);
        }
    }
}
