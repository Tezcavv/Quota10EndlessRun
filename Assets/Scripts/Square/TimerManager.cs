using System;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private float timerDuration = 20f;

    [Header("UI")]
    [SerializeField] private GameObject timerDisplayUI;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Animation")]
    [SerializeField] private float bounceScale = 1.2f;
    [SerializeField] private float bounceDuration = 0.25f;

    private float currentTime;
    private bool timerRunning;
    private bool isBouncing;

    public static Action OnTimerEnd;

    private void OnEnable()
    {
        EntrySquarePoint.OnPlayerEnterOnEntryPoint += StartTimer;
        ExitSquarePoint.OnPlayerEnterOnExitPoint += StopTimer;
    }

    private void OnDisable()
    {
        EntrySquarePoint.OnPlayerEnterOnEntryPoint -= StartTimer;
        ExitSquarePoint.OnPlayerEnterOnExitPoint -= StopTimer;
    }

    private void Update()
    {
        if (!timerRunning)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            UpdateTimerText();
            TimerEnded();
            return;
        }

        UpdateTimerText();

        if (currentTime <= 5f)
            PlayBounce();
    }

    private void StartTimer()
    {
        currentTime = timerDuration;
        timerRunning = true;
        isBouncing = false;

        timerDisplayUI.SetActive(true);
        timerText.transform.localScale = Vector3.one;

        UpdateTimerText();
    }

    private void StopTimer()
    {
        timerRunning = false;
        timerDisplayUI.SetActive(false);

        DOTween.Kill(timerText.transform);
        timerText.transform.localScale = Vector3.one;
    }

    private void TimerEnded()
    {
        timerRunning = false;
        timerDisplayUI.SetActive(false);

        OnTimerEnd?.Invoke();
    }

    private void UpdateTimerText()
    {
        int seconds = Mathf.CeilToInt(currentTime);
        timerText.text = seconds.ToString();
    }

    private void PlayBounce()
    {
        if (isBouncing)
            return;

        isBouncing = true;

        timerText.transform
            .DOPunchScale(Vector3.one * (bounceScale - 1f), bounceDuration)
            .OnComplete(() => isBouncing = false);
    }
}
