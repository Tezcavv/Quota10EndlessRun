using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
   public GameObject panel;
   public RectTransform text;
   public Image left;
   public Image right;
   public Button returnToMainMenuBtn;
   public CanvasGroup fade;
   
   public PlayerController_Endless endlessPlayerController;
   public ScoreManager scoreManager;
   private Tween t;
   
   public TextMeshProUGUI highScoreTxt;
   public Vector3 punch = new(2, 2, 2);
   public void ActivateGameOver()
   {
      scoreManager.SetHighScore();
      DOTween.To(
            () => Time.timeScale,
            x => Time.timeScale = x,
            0f,
            2f
         )
         .SetEase(Ease.OutQuad)      
         .SetUpdate(true);    
      panel.SetActive(true);
      fade.DOFade(1, 0.8f).SetUpdate(true);
      t?.Kill(false);
      t = text.DOAnchorPos(new Vector2(0, 0), 1.2f).SetEase(Ease.InOutElastic).SetUpdate(UpdateType.Normal, true).OnComplete(() =>
      {
         highScoreTxt.text = "Run Score: " + scoreManager.GetCurrentScoreTheatre();
         highScoreTxt.gameObject.SetActive(true);
         highScoreTxt.GetComponent<RectTransform>().DOPunchScale(punch, 1).SetUpdate(true);
         returnToMainMenuBtn.gameObject.SetActive(true);
         returnToMainMenuBtn.GetComponent<CanvasGroup>().DOFade(1f, 1.2f).SetUpdate(true);
      });
      
   }

   private void Awake()
   {
      endlessPlayerController.OnPlayerDeath += ActivateGameOver;
      returnToMainMenuBtn.onClick.AddListener(() => SceneManager.LoadScene(0));
      returnToMainMenuBtn.gameObject.SetActive(false);
      returnToMainMenuBtn.GetComponent<CanvasGroup>().alpha = 0;
      fade.alpha = 0;
   }

   private void OnDestroy()
   {
      endlessPlayerController.OnPlayerDeath -= ActivateGameOver;
      returnToMainMenuBtn.onClick.RemoveAllListeners();
   }

  
   
}
