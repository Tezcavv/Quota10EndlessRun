using UnityEngine;

public class PlayerSFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceWalk;

    [SerializeField] private AudioClip[] walkSFX;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip[] hurtSFX;

    private float counterWalkSFX = 0f;
    private float delayWalkSFX = 0.7f;

    private void Update()
    {
        // Fai partire il suoni di camminata ogni delayWalkSFX secondi
        counterWalkSFX += Time.deltaTime;
        if (counterWalkSFX >= delayWalkSFX)
        {
            PlayWalkSFX();
            counterWalkSFX = 0f;
        }
    }

    public void PlayWalkSFX()
    {
        if (walkSFX.Length == 0) return;
        AudioClip clip = walkSFX[Random.Range(0, walkSFX.Length)];
        audioSource.PlayOneShot(clip);
    }

    public void PlayJumpSFX()
    {
        if (jumpSFX == null) return;
        audioSource.PlayOneShot(jumpSFX);
    }

    public void PlayHurtSFX()
    {
        if (hurtSFX.Length == 0) return;
        AudioClip clip = hurtSFX[Random.Range(0, hurtSFX.Length)];
        audioSource.PlayOneShot(clip);
    }
}
