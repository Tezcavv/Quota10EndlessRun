using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RunSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource runAudioSource;
    [SerializeField] private AudioClip endlessClip;
    [SerializeField] private AudioClip squareClip;
    [SerializeField] private float fadeDuration = 1.0f;

    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private AudioSource sfxAudioSource;

    [SerializeField] private GroundGenerationManager groundGenerationManager;

    private void Awake()
    {
        runAudioSource.loop = true;

        // Start with endless mode clip
        runAudioSource.clip = endlessClip;
        runAudioSource.Play();
        runAudioSource.DOFade(1f, fadeDuration).SetEase(Ease.InOutQuad);
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
        StartCoroutine(FadeOutAndIn(endlessClip, fadeDuration));
    }

    public void SwitchToSquareModeWithFade()
    {
        Debug.Log("Switching to Square Mode with Fade");
        StartCoroutine(FadeOutAndIn(squareClip, fadeDuration));
    }

    private IEnumerator FadeOutAndIn(AudioClip newClip, float duration)
    {
        runAudioSource.DOFade(0f, duration).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(duration);
        runAudioSource.clip = newClip;
        runAudioSource.Play();
        runAudioSource.DOFade(1f, duration).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(duration);
    }

    public void PlayClickSound()
    {
        sfxAudioSource.PlayOneShot(clickSFX);
    }
}
