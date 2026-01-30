using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private CanvasGroup cv;
    
    public void StartGame()
    {
        cv.DOFade(1f,0.8f).SetEase(Ease.OutQuad).OnComplete(() => SceneManager.LoadScene(1));
    }

    
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    
    
    
}
