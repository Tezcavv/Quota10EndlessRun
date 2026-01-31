using UnityEngine;

public class PlayerSFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] walkSFX;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip[] hurtSFX;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
