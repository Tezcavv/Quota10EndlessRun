using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RunSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource endlessAudioSource;
    [SerializeField] private AudioSource sqareAudioSource;
    [SerializeField] private AudioClip endlessClip;
    [SerializeField] private AudioClip squareClip;
    [SerializeField] private float fadeDuration = 1.0f;

    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioSource sfxAudioSource;

    [SerializeField] private GroundGenerationManager groundGenerationManager;

    private void Awake()
    {
        endlessAudioSource.loop = true;
        sqareAudioSource.loop = true;

        // Start both sounds but set volumte to 0 initially for square mode
        endlessAudioSource.clip = endlessClip;
        sqareAudioSource.clip = squareClip;
        endlessAudioSource.volume = 0.0f;
        sqareAudioSource.volume = 0.0f;
        endlessAudioSource.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad);
        endlessAudioSource.Play();
        sqareAudioSource.Play();
    }

    private void OnEnable()
    {
        groundGenerationManager.OnEnterEndlessMode += SwitchToEndlessModeWithFade;
        groundGenerationManager.OnEnterSquareMode += SwitchToSquareModeWithFade;
    }

    private void OnDisable()
    {
        groundGenerationManager.OnEnterEndlessMode -= SwitchToEndlessModeWithFade;
        groundGenerationManager.OnEnterSquareMode -= SwitchToSquareModeWithFade;
    }

    public void SwitchToEndlessModeWithFade()
    {
        Debug.Log("Switching to Endless Mode with Fade");
        endlessAudioSource.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad);
        sqareAudioSource.DOFade(0f, fadeDuration).SetEase(Ease.InOutQuad);
    }

    public void SwitchToSquareModeWithFade()
    {
        Debug.Log("Switching to Square Mode with Fade");
        endlessAudioSource.DOFade(0f, fadeDuration).SetEase(Ease.InOutQuad);
        sqareAudioSource.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad);
    }

    public void PlayClickSound()
    {
        sfxAudioSource.Stop();
        sfxAudioSource.clip = clickSFX;
        sfxAudioSource.Play();
    }
}
