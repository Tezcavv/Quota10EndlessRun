using System;
using UnityEngine;


  public class PauseManager : MonoBehaviour
  {

    public static bool IsPaused { get; private set; } = false;

    public static void TogglePause()
    {
      IsPaused = !IsPaused;
      Time.timeScale = IsPaused ? 0f : 1f;
    }

    public static void Pause()
    {
      IsPaused = true;
      Time.timeScale = IsPaused ? 0f : 1f;
    }

    public static void Unpause()
    {
      IsPaused = false;
      Time.timeScale = IsPaused ? 0f : 1f;
    }

    private void Awake()
    {
      DontDestroyOnLoad(gameObject);
    }
  }
