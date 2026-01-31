using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{

    [SerializeField] public GameObject panel;
    [SerializeField] public GameObject tutorial;
    [SerializeField] public GameObject options;


    public void Resume()
    {
        panel.SetActive(false);
        PauseManager.Unpause();
    }

    public void OpenTutorial()
    {
        tutorial.SetActive(true);
    }
    
    public void OpenOptions()
    {
       options.SetActive(true);
    }

    public void OpenOnlyFans()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    private bool IsAnyWindowOpen()
    {
        return tutorial.activeSelf && options.activeSelf;
    }

    void OpenPauseMenu()
    {

        PauseManager.Pause();
        panel.SetActive(true);
    }

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OpenPauseMenu();
        }
    }
}

