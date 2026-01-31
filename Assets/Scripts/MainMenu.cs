using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup cv;
    [SerializeField] private GameObject optionsMenu;

    [SerializeField] private MainMenuSoundManager soundManager;

    private bool isLoading = false;

    private void Awake()
    {
        cv.alpha = 0f;
        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        DOTween.Kill(cv);
    }

    public void StartGame()
    {
        if (isLoading) return;
        isLoading = true;

        soundManager.PlayMainMenuOSTFade(0f, 0.8f);

        DOTween.Kill(cv);

        cv.alpha = 0f; 

        cv.DOFade(1f, 0.8f)
          .SetEase(Ease.OutQuad)
          .OnComplete(() => SceneManager.LoadScene(1));
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
