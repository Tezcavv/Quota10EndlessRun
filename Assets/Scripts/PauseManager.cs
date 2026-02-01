using System;
using UnityEngine;


  public class PauseManager : MonoBehaviour
  {

    public bool IsPaused { get; private set; } = false;

    public static PauseManager instance;

    private void Awake()
    {
      instance = this;
      Unpause();
    }



    public void Pause()
    {
      IsPaused = true;
      Time.timeScale = IsPaused ? 0f : 1f;
    }

    public void Unpause()
    {
      IsPaused = false;
      Time.timeScale = IsPaused ? 0f : 1f;
    }

    private void OnDestroy()
    {
      Unpause();
    }
  }
