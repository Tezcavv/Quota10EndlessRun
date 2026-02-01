using UnityEngine;

public class PlayerSFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceWalk;

    [SerializeField] private AudioClip[] walkSFX;
    [SerializeField] private AudioClip jumpSFX;
    [SerializeField] private AudioClip[] hurtSFX;

    [SerializeField] private float counterWalkSFX = 0f;
    [SerializeField] private float delayWalkSFX = 0.7f;

    //private void Update()
    //{
    //    // Fai partire il suoni di camminata ogni delayWalkSFX secondi
    //    counterWalkSFX += Time.deltaTime;
    //    if (counterWalkSFX >= delayWalkSFX)
    //    {
    //        PlayWalkSFX();
    //        counterWalkSFX = 0f;
    //    }
    //}

    private void Start()
    {
        audioSourceWalk.loop = true;
        PlayWalkSFX();
    }

    public void PlayWalkSFX()
    {
        //if (walkSFX.Length == 0) return;
        //AudioClip clip = walkSFX[Random.Range(0, walkSFX.Length)];
        //audioSourceWalk.Stop();
        //audioSourceWalk.clip = clip;
        //audioSourceWalk.Play();

        AudioClip clip = walkSFX[0];
        audioSourceWalk.Stop();
        audioSourceWalk.clip = clip;
        audioSourceWalk.Play();
    }

    public void PlayJumpSFX()
    {
        if (jumpSFX == null) return;
        audioSource.Stop();
        audioSource.clip = jumpSFX;
        audioSource.Play();
    }

    public void PlayHurtSFX()
    {
        if (hurtSFX.Length == 0) return;
        AudioClip clip = hurtSFX[Random.Range(0, hurtSFX.Length)];
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
