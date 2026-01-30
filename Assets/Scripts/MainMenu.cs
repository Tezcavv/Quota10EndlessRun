using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private CanvasGroup cv;
    [SerializeField] private GameObject optionsMenu;
    
    public void StartGame()
    {
        cv.DOFade(1f,0.8f).SetEase(Ease.OutQuad).OnComplete(() => SceneManager.LoadScene(1));
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }


}
