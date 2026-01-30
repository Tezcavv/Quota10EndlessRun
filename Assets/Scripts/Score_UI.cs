using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score_UI : MonoBehaviour
{
    [SerializeField]Canvas scoreCanvas;
    [SerializeField] TextMeshProUGUI scoreTextCart, scoreTextTheatre;
    [SerializeField]ScoreManager highScoreCounter;

    void Start()
    {
        scoreCanvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreTextCart.text = "Cart Score: " + highScoreCounter.GetCurrentScoreCart().ToString();
        scoreTextTheatre.text = "Theatre Score: " + highScoreCounter.GetCurrentScoreTheatre().ToString();
    }
}
