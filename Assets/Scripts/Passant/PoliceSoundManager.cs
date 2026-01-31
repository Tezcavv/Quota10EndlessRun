using UnityEngine;

public class PoliceSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip whistle;
    [SerializeField] private AudioClip hey;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayRandomSound();
    }

    private void PlayRandomSound()
    {
        float delay = Random.Range(5f, 15f);
        Invoke("PlayRandomSound", delay);
        if (Random.value < 0.5f)
        {
            PlayWhistle();
        }
        else
        {
            PlayHey();
        }

        PlayRandomSound();
    }

    public void PlayWhistle()
    {
        if (whistle == null) return;
        audioSource.PlayOneShot(whistle);
    }

    public void PlayHey()
    {
        if (hey == null) return;
        audioSource.PlayOneShot(hey);
    }
}
