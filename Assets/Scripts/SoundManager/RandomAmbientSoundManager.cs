using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAmbientSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] ambientClips;

    [Header("Delay Range (seconds)")]
    [SerializeField] private float minDelay = 5f;
    [SerializeField] private float maxDelay = 15f;

    [SerializeField] private PlayerController_Endless playerController_Endless;

    private AudioSource ambientAudioSource;
    private Coroutine playRoutine;

    private void Awake()
    {
        ambientAudioSource = GetComponent<AudioSource>();
        ambientAudioSource.loop = false;
        ambientAudioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        playRoutine = StartCoroutine(PlayRandomAmbientLoop());
        playerController_Endless.OnPlayerDeath += HandleIsDeath;
        TimerManager.OnTimerEnd += HandleIsDeath;
    }

    private void OnDisable()
    {
        if (playRoutine != null)
            StopCoroutine(playRoutine);
        playerController_Endless.OnPlayerDeath -= HandleIsDeath;
        TimerManager.OnTimerEnd -= HandleIsDeath;
    }

    private IEnumerator PlayRandomAmbientLoop()
    {
        while (true)
        {
            if (ambientClips.Length == 0)
                yield break;

            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            AudioClip clip = ambientClips[Random.Range(0, ambientClips.Length)];
            ambientAudioSource.Stop();
            ambientAudioSource.clip = clip;
            ambientAudioSource.Play();
        }
    }

    private void HandleIsDeath()
    {
        ambientAudioSource.Stop();
        if (playRoutine != null)
            StopCoroutine(playRoutine);
    }
}
