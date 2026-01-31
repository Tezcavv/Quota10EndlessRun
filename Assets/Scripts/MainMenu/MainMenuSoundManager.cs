using DG.Tweening;
using UnityEngine;

public class MainMenuSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip mainMenuOST;
    [SerializeField] private AudioClip clickAudioClip;
    [SerializeField] private AudioSource ostAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    private void Start()
    {
        ostAudioSource.volume = 0f;
        ostAudioSource.clip = mainMenuOST;
        ostAudioSource.loop = true;
        PlayMainMenuOSTFade(1f, 1f);
    }

    public void PlayMainMenuOSTFade(float targetVolume, float duration)
    {
        ostAudioSource.Play();
        ostAudioSource.DOFade(targetVolume, duration).SetEase(Ease.InOutQuad);
    }

    public void PlayClickSound()
    {
        sfxAudioSource.PlayOneShot(clickAudioClip);
    }
}
