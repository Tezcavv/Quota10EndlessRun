using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PoliceSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip whistle;
    [SerializeField] private AudioClip hey;

    private AudioSource audioSource;


    private float currTimeInState = 0;
    private float delay = 5;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
       delay = Random.Range(5f, 15f);
    }

    private void Update()
    {
        
        currTimeInState += Time.deltaTime;
        if (currTimeInState >= delay)
        {
            currTimeInState = 0;
            delay = Random.Range(5f, 15f);
            if (Random.value < 0.5f)
            {
                PlayWhistle();
            }
            else
            {
                PlayHey();
            }
        }
        
    }


    public void PlayWhistle()
    {
        if (whistle == null) return;
        audioSource.Stop();
        audioSource.clip = whistle;
        audioSource.Play();
    }

    public void PlayHey()
    {
        if (hey == null) return;
        audioSource.Stop();
        audioSource.clip = hey;
        audioSource.Play();
    }
}
