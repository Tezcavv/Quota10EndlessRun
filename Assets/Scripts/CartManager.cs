using System;
using System.Collections.Generic;
using UnityEngine;

public class CartManager : MonoBehaviour
{
    public static event Action<Passant> OnCartCollided = delegate { };
    public static event Action OnEnteringTheatre = delegate { };

    [SerializeField, Min(0)] private int bodies;
    [Tooltip("How many points should be reached each time to show more bodies on cart?")]
    [SerializeField, Min(1)] private int showBodyFactor = 1;
    [SerializeField] private List<MeshRenderer> bodyRenderers;

    [SerializeField] private AudioClip[] swoosh; // Sound played when colliding with a passant
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        ScoreManager.OnCartScoreChanged += HandleBodyRenderers;
        ScoreManager.OnTheaterScoreChanged += HandleBodyRenderers;
        //theater.onPlayerEnterTheatre +=  enetringTheatre;
    }

    private void Start()
    {
        HandleBodyRenderers(0);
    }

    private void OnDestroy()
    {
        ScoreManager.OnCartScoreChanged -= HandleBodyRenderers;
        ScoreManager.OnTheaterScoreChanged -= HandleBodyRenderers;
        //theater.onPlayerEnterTheatre -=  enetringTheatre;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Sbattuto con " +  collision.gameObject.name);
        Passant hitPassant = collision.gameObject.GetComponentInParent<Passant>();
        if (hitPassant)
        {
            hitPassant.gameObject.SetActive(false);

            if (hitPassant.PassInfo.type != PassInfo.PassType.police)
            {
                PlaySwooshSound();
                bodies++;
                Debug.Log("Corpo aggiunto al carrello. Totale corpi: " + bodies);
            }
            OnCartCollided?.Invoke(hitPassant);
        }
    }

    private void HandleBodyRenderers(int newScore)
    {
        Debug.Log("Handle corpi: " + newScore);
        if(newScore <= 0)
        {
            bodies = 0;
        }
        int amountToShow = bodies;// % showBodyFactor;
        for(int i = 0; i < bodyRenderers.Count; i++)
        {
            bodyRenderers[i].enabled = i + 1 < amountToShow;
        }
    }

    private void EnteringTheatre()
    {
        OnEnteringTheatre?.Invoke();
        bodies = 0;
        Debug.Log("Entrando nel teatro. Corpi azzerati.");
    }

    private void PlaySwooshSound()
    {
        if (swoosh.Length == 0) return;
        AudioClip clip = swoosh[UnityEngine.Random.Range(0, swoosh.Length)];
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}
